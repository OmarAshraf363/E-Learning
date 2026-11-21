const { chromium } = require('playwright');
const path = require('path');
const out = process.argv[2] || path.join(__dirname, 'screenshots');
require('fs').mkdirSync(out, { recursive: true });
const base = process.env.BASE_URL || 'http://localhost:4200';
const png = { name: 'cover.png', mimeType: 'image/png', buffer: Buffer.from('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==', 'base64') };
(async () => {
  const b = await chromium.launch();
  const p = await b.newPage({ viewport: { width: 1366, height: 900 } });
  const errors = [];
  p.on('console', (m) => m.type() === 'error' && errors.push(m.text()));
  p.on('pageerror', (e) => errors.push('PAGEERROR ' + e.message));
  const step = (s) => console.log('✔', s);
  p.on('response', (r) => r.url().includes('academic') && console.log('   ', r.status(), r.request().method(), r.url()));
  const stamp = Date.now().toString().slice(-5);
  const noSwal = () => p.waitForSelector('.swal2-container', { state: 'detached', timeout: 10000 });

  await noSwal();
  await p.goto(base + '/auth/login');
  await p.fill('#email', 'Admin@gmail.com'); await p.fill('#password', 'Admin@123'); await p.click('#login-submit');
  await p.waitForURL('**/admin');
  await p.waitForSelector('text=Total Courses');
  await p.waitForTimeout(800);
  await p.screenshot({ path: `${out}/p2-dashboard.png` });
  step('dashboard: ' + (await p.locator('.card-title').allInnerTexts()).join(' | '));

  // Department create (modal) -> edit without cover
  const dept = `Dept${stamp}`;
  await noSwal();
  await p.goto(base + '/admin/departments');
  await p.click('text=New Department');
  await p.fill('#d-name', dept); await p.fill('#d-desc', 'Department description');
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector('text=Please upload an image.');
  step('department cover required on create');
  await p.setInputFiles('#d-cover', png);
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector(`td:text("${dept}")`);
  step('department created');
  const row = p.locator('tr', { hasText: dept });
  await noSwal();
  await row.locator('button:has-text("Edit")').click();
  await p.waitForFunction((n) => document.querySelector('#d-name')?.value === n, dept);
  await p.fill('#d-name', dept + 'X');
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector(`td:text("${dept}X")`);
  step('department edited');
  await noSwal();
  await p.locator('tr', { hasText: dept + 'X' }).locator('a:has-text("Details")').click();
  await p.waitForSelector(`text=${dept}X Details`);
  step('department details page');

  // Track create
  const track = `Track${stamp}`;
  await noSwal();
  await p.goto(base + '/admin/tracks');
  await p.click('text=New Track');
  await p.fill('#t-name', track); await p.fill('#t-desc', 'Track description'); await p.setInputFiles('#t-cover', png);
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector(`h5:text("${track}")`);
  step('track created');

  // Course create with lists
  const course = `Course${stamp}`;
  await noSwal();
  await p.goto(base + '/admin/courses/new');
  await p.click('button:has-text("Create")');
  await p.waitForSelector('text=This field is required.');
  step('course client validation');
  await p.fill('#courseName', course); await p.fill('#description', 'Course description');
  await p.setInputFiles('#imgCover', png);
  await p.fill('#price', '150'); await p.fill('#credits', '3');
  await p.fill('#demoVideoUrl', 'https://example.com/v.mp4');
  await p.fill('#startDate', '2025-02-01'); await p.fill('#endDate', '2025-06-01');
  await p.selectOption('#instructorId', { label: 'Instructor One' });
  await p.selectOption('#trackId', { label: track });
  await p.selectOption('#departmentId', { label: dept + 'X' });
  await p.click('text=Add Key Word'); await p.click('text=Add Key Word');
  const kw = p.locator('input[placeholder="Enter Key Word"]');
  await kw.nth(0).fill('angular'); await kw.nth(1).fill('signals');
  await p.click('text=Add Objective'); await p.fill('textarea[placeholder="Enter learning objective"]', 'Build SPAs');
  await p.click('text=Add Topic'); await p.fill('textarea[placeholder="Enter topic"]', 'Routing');
  await p.click('button:has-text("Create")');
  await p.waitForURL('**/admin/courses');
  await p.waitForSelector(`td:text("${course}")`);
  step('course created with keywords/objectives/topics');

  await noSwal();
  await p.locator('tr', { hasText: course }).locator('a:has-text("Edit")').click();
  await p.waitForFunction((n) => document.querySelector('#courseName')?.value === n, course);
  const kwValues = await p.locator('input[placeholder="Enter Key Word"]').evaluateAll((els) => els.map((e) => e.value));
  if (kwValues.join(',') !== 'angular,signals') throw new Error('keywords not loaded: ' + kwValues);
  await p.screenshot({ path: `${out}/p2-course-form.png`, fullPage: true });
  await p.locator('input[placeholder="Enter Key Word"]').nth(1).locator('xpath=..').locator('button').click();
  await p.click('button:has-text("Update")');
  await p.waitForURL('**/admin/courses');
  await noSwal();
  await p.locator('tr', { hasText: course }).locator('a:has-text("Edit")').click();
  await p.waitForFunction((n) => document.querySelector('#courseName')?.value === n, course);
  const kw2 = await p.locator('input[placeholder="Enter Key Word"]').evaluateAll((els) => els.map((e) => e.value));
  if (kw2.join(',') !== 'angular') throw new Error('keyword removal failed: ' + kw2);
  if (!(await p.locator('img[alt="Current cover"]').count())) throw new Error('cover lost after edit');
  step('course edited: keyword removed, cover kept');

  // Track details shows the course
  await noSwal();
  await p.goto(base + '/admin/tracks');
  await p.locator('.one-track', { hasText: track }).locator('a:has-text("View")').click();
  await p.waitForSelector(`dd:text("${course}")`);
  step('track details lists course');

  // Academic year
  await noSwal();
  await p.goto(base + '/admin/academic-years');
  await p.waitForSelector('td:text("2020-2021")');
  await p.click('text=Add New Academic Year');
  await p.waitForSelector('#yearName');
  await p.fill('#yearName', `20${stamp.slice(0, 2)}-X`);
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector(`td:text("20${stamp.slice(0, 2)}-X")`);
  step('academic year saved');

  // Schedule with instructor
  await noSwal();
  await p.goto(base + '/admin/schedules/new');
  await p.selectOption('#courseId', { label: course });
  await p.selectOption('#instructorId', { label: 'instructor1' });
  await p.selectOption('#roomId', { label: 'Lab 202' });
  await p.selectOption('#dayOfWeek', 'Wednesday');
  await p.fill('#startTime', '09:30'); await p.fill('#endTime', '11:00');
  await p.click('text=Save Schedule');
  await p.waitForURL('**/admin/schedules');
  await p.waitForSelector(`td:text("${course}")`);
  await p.screenshot({ path: `${out}/p2-schedules.png` });
  step('schedule with instructor created');

  // Enrollments
  await noSwal();
  await p.goto(base + '/admin/enrollments');
  await p.fill('#email', 'nobody@nowhere.com'); await p.selectOption('#course', { label: course });
  await p.click('text=Enroll via Email');
  await p.waitForSelector('text=Student with this email does not exist.');
  await p.fill('#email', 'student1@example.com'); await p.selectOption('#course', { label: course });
  await p.click('text=Enroll via Email');
  await p.waitForSelector('text=Student has been successfully enrolled.');
  step('enroll by email (error + success)');
  await noSwal();
  await p.click('text=Enroll via CSV File');
  await p.setInputFiles('#csvFile', { name: 's.csv', mimeType: 'text/csv', buffer: Buffer.from('1,1\nbad\n') });
  await p.click('.modal-body button:has-text("Upload")');
  await p.waitForSelector('text=1 enrollment(s) added, 1 line(s) skipped.');
  step('CSV import');

  console.log('console errors:', errors.filter((e) => !/status of (400|404|409)/.test(e)));
  await b.close();
})().catch(async (e) => { console.error('FAILED', e.message); process.exit(1); });
