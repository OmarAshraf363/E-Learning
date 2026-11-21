const { chromium } = require('playwright');
const path = require('path');
const out = process.argv[2] || path.join(__dirname, 'screenshots');
require('fs').mkdirSync(out, { recursive: true });
const base = process.env.BASE_URL || 'http://localhost:4200';
(async () => {
  const b = await chromium.launch();
  const ctx = await b.newContext({ viewport: { width: 1366, height: 900 } });
  const p = await ctx.newPage();
  const errors = [];
  p.on('console', (m) => m.type() === 'error' && errors.push(m.text()));
  p.on('pageerror', (e) => errors.push('PAGEERROR ' + e.message));
  const step = (s) => console.log('✔', s);
  const noSwal = () => p.waitForSelector('.swal2-container', { state: 'detached', timeout: 10000 });

  // Anonymous home
  await p.goto(base + '/');
  await p.waitForSelector('text=Top Courses for You');
  await p.waitForSelector('.one-course');
  await p.waitForTimeout(800);
  await p.screenshot({ path: `${out}/p3-home.png` });
  const cards = await p.locator('.one-course').count();
  step(`home rendered (${cards} course cards)`);

  // hover popover
  const first = p.locator('#courses .one-course').first();
  await first.hover();
  await p.waitForSelector('#courses .modal-test >> visible=true');
  step('hover popover opens');
  await p.mouse.move(5, 5);

  // category tab
  const tab = p.locator('.category-list .btn').nth(1);
  const tabName = await tab.innerText();
  await tab.click();
  await p.waitForFunction((n) => [...document.querySelectorAll('.category-list .btn.active')].some((b) => b.textContent.trim() === n), tabName.trim());
  step('category tab switched: ' + tabName.trim());

  // navbar categories menu + search
  await p.hover('text=Categories | Tracks');
  await p.waitForSelector('.dropdown-submenu >> visible=true');
  step('navbar categories menu');
  await p.fill('header input[type=search]', 'digital');
  await p.press('header input[type=search]', 'Enter');
  await p.waitForURL('**/courses?search=digital');
  await p.waitForSelector('text=Courses that have');
  await p.waitForSelector('#courses .course-title h5');
  const titles = await p.locator('.course-title h5').allInnerTexts();
  if (!titles.length || !titles.every((t) => t.toLowerCase().includes('digital'))) throw new Error('search not applied: ' + titles);
  step('search filter applied: ' + titles.join(', '));

  // filters
  await p.goto(base + '/courses');
  await p.waitForSelector('#category-1');
  await p.check('#category-1');
  await p.click('text=Apply Filters');
  await p.waitForURL('**categoryId=1**');
  await p.waitForTimeout(800);
  const catTitles = await p.locator('.course-title h5').allInnerTexts();
  if (catTitles.includes('Digital Circuits')) throw new Error('category filter not applied');
  step('category filter applied (' + catTitles.length + ' courses)');

  // course details + reviews filter
  await p.goto(base + '/courses/1');
  await p.waitForSelector('text=About the Instructor');
  await p.waitForSelector('text=Based on');
  await p.click('.filter-btn:has-text("5 Stars")');
  await p.waitForURL('**starFilter=5');
  await p.click('text=📚 Topics Cover');
  step('course details: reviews filter + topics tab');
  await p.screenshot({ path: `${out}/p3-details.png`, fullPage: false });

  // categories + category + topic
  await p.goto(base + '/category');
  await p.waitForSelector('.oneDepart');
  await p.locator('.oneDepart a:has-text("Explore")').first().click();
  await p.waitForSelector('text=Top Courses for You in');
  step('category page');
  await p.goto(base + '/topic/program');
  await p.waitForSelector('.course-title h5:text("Introduction to Programming")');
  step('topic page');

  // Teach on BFCAI modal (instructor self registration)
  await p.goto(base + '/');
  await p.click('text=Teach on BFC');
  await p.waitForSelector('text=Teach In BFCAI');
  const email = `teach${Date.now()}@test.com`;
  await p.fill('#ir-fullName', 'New Teacher'); await p.fill('#ir-email', email);
  await p.fill('#ir-password', 'Pass@123'); await p.fill('#ir-confirm', 'Pass@123');
  await p.click('.modal-body button[type=submit]');
  await p.waitForSelector('text=Your instructor account was created');
  step('teach-on-BFCAI modal registered an instructor');
  await noSwal();

  // Instructor login -> redirected away from public home
  await p.goto(base + '/auth/login');
  await p.fill('#email', email); await p.fill('#password', 'Pass@123'); await p.click('#login-submit');
  await p.waitForURL('**/instructor');
  await p.goto(base + '/');
  await p.waitForURL('**/instructor');
  step('instructor is redirected from / to /instructor (legacy Home redirect)');

  console.log('console errors:', errors.filter((e) => !/status of (400|401|404)/.test(e)));
  await b.close();
})().catch((e) => { console.error('FAILED', e.message); process.exit(1); });
