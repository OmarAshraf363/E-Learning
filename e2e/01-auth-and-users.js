const { chromium } = require('playwright');
const path = require('path');
const out = process.argv[2] || path.join(__dirname, 'screenshots');
require('fs').mkdirSync(out, { recursive: true });
const base = process.env.BASE_URL || 'http://localhost:4200';
(async () => {
  const b = await chromium.launch();
  const ctx = await b.newContext({ viewport: { width: 1280, height: 800 } });
  const p = await ctx.newPage();
  const errors = [];
  p.on('console', (m) => m.type() === 'error' && errors.push(m.text()));
  const step = (s) => console.log('✔', s);

  // 1) Admin login -> /admin
  await p.goto(base + '/auth/login');
  await p.screenshot({ path: `${out}/p1-login.png` });
  await p.fill('#email', 'Admin@gmail.com');
  await p.fill('#password', 'Admin@123');
  await p.click('#login-submit');
  await p.waitForURL('**/admin');
  step('admin login -> ' + p.url());

  // 2) Create a student from admin
  const email = `e2e${Date.now()}@test.com`;
  await p.goto(base + '/admin/users?filter=Student');
  await p.click('text=Add New User');
  await p.waitForURL('**/admin/users/new**');
  await p.fill('#fullName', 'E2E Student');
  await p.fill('#userName', email);
  await p.fill('#email', email);
  await p.fill('#password', 'Pass@123');
  await p.selectOption('#role', 'Student');
  await p.click('button[type=submit]');
  await p.waitForURL('**/admin/users?filter=Student');
  await p.waitForSelector(`td:text("${email}")`);
  step('admin created student');
  await p.waitForTimeout(2000);
  await p.screenshot({ path: `${out}/p1-users.png` });

  // 3) Lock that user
  const row = p.locator('tr', { hasText: email });
  await row.locator('button:has-text("Lock")').click();
  await row.locator('button:has-text("Unlock")').waitFor();
  step('locked user');

  // 3b) Instructor linked to a department (second plan); the department list is for instructors only
  const instEmail = `inst${Date.now()}@test.com`;
  await p.goto(base + '/admin/users/new?role=Instructor');
  await p.fill('#userName', instEmail); await p.fill('#email', instEmail); await p.fill('#password', 'Pass@123');
  await p.selectOption('#departmentId', { label: 'Electrical Engineering' });
  await p.selectOption('#role', 'Student');
  if (await p.locator('#departmentId').count()) throw new Error('department should be hidden for students');
  await p.selectOption('#role', 'Instructor');
  await p.selectOption('#departmentId', { label: 'Electrical Engineering' });
  await p.click('button[type=submit]');
  await p.waitForURL('**/admin/users?filter=Instructor');
  await p.locator('tr', { hasText: instEmail }).locator('a[title=Edit]').click();
  await p.waitForFunction(() => document.querySelector('#departmentId')?.selectedOptions[0]?.text === 'Electrical Engineering');
  step('instructor created with a department (shown again on edit)');

  // 3c) The admin can't delete or lock their own account (buttons hidden, API refuses)
  await p.goto(base + '/admin/users');
  const me = p.locator('tr', { hasText: 'Admin@gmail.com' });
  await me.waitFor();
  if (await me.locator('button').count()) throw new Error('own row must not have delete / lock buttons');
  step('no delete / lock on the admin\'s own row');

  // 4) Logout, locked user cannot log in
  await p.click('button[title=Logout]');
  await p.waitForURL('**/auth/login');
  await p.fill('#email', email);
  await p.fill('#password', 'Pass@123');
  await p.click('#login-submit');
  await p.waitForSelector('text=User account locked out.');
  step('locked user blocked: "User account locked out."');

  // 5) Visitor registers -> student lands on public home
  const email2 = `reg${Date.now()}@test.com`;
  await p.goto(base + '/auth/register');
  await p.fill('#fullName', 'Reg Student');
  await p.fill('#email', email2);
  await p.fill('#password', 'abc');
  await p.fill('#confirmPassword', 'abcd');
  await p.click('#registerSubmit');
  await p.waitForSelector('text=The password and confirmation password do not match.');
  step('client validation shown');
  await p.fill('#password', 'abcdef');
  await p.fill('#confirmPassword', 'abcdef');
  await p.click('#registerSubmit');
  await p.waitForSelector('text=Passwords must have at least one');
  step('server (Identity) password rules shown');
  await p.screenshot({ path: `${out}/p1-register-errors.png` });
  await p.fill('#password', 'Pass@123');
  await p.fill('#confirmPassword', 'Pass@123');
  await p.click('#registerSubmit');
  await p.waitForURL(base + '/');
  step('registered + signed in -> public home');

  // 6) Student cannot open /admin -> redirected to home
  await p.goto(base + '/admin');
  await p.waitForURL(base + '/');
  step('student blocked from /admin');

  // 7) Profile update
  await p.goto(base + '/account');
  await p.fill('#phoneNumber', '01000000000');
  await p.fill('#jopDescription', 'Computer science student');
  await p.setInputFiles('#profilePicture', { name: 'me.png', mimeType: 'image/png',
    buffer: Buffer.from('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==', 'base64') });
  await p.click('text=Save Changes');
  await p.waitForSelector('text=Your profile has been updated');
  step('profile updated');
  await p.waitForTimeout(2000);
  await p.screenshot({ path: `${out}/p1-profile.png` });

  // 8) Access token refresh: break the access token, next call must refresh transparently
  await p.evaluate(() => { const s = JSON.parse(localStorage.getItem('bfcai.auth')); s.accessToken = 'broken'; localStorage.setItem('bfcai.auth', JSON.stringify(s)); });
  await p.goto(base + '/account');
  await p.waitForTimeout(2500);
  const phone = await p.inputValue('#phoneNumber');
  console.log('   phone after reload =', JSON.stringify(phone));
  if (phone !== '01000000000') throw new Error('profile not loaded after refresh');
  step('expired/invalid access token refreshed automatically');

  console.log('console errors:', errors.filter(e => !e.includes('401') && !e.includes('400')));
  await b.close();
})().catch((e) => { console.error('FAILED', e.message); process.exit(1); });
