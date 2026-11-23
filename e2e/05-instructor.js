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
  const noSwal = () => p.waitForSelector('.swal2-container', { state: 'detached', timeout: 10000 });
  const stamp = Date.now().toString().slice(-5);

  // Become an instructor through "Teach on BFCAI"
  const email = `inst${stamp}@test.com`;
  await p.goto(base + '/');
  await p.click('text=Teach on BFC');
  await p.fill('#ir-fullName', 'Teacher ' + stamp); await p.fill('#ir-email', email);
  await p.fill('#ir-password', 'Pass@123'); await p.fill('#ir-confirm', 'Pass@123');
  await p.click('.modal-body button[type=submit]');
  await p.waitForSelector('text=Your instructor account was created');
  await noSwal();
  await p.goto(base + '/auth/login');
  await p.fill('#email', email); await p.fill('#password', 'Pass@123'); await p.click('#login-submit');
  await p.waitForURL('**/instructor');
  await p.waitForSelector('text=Instructor Dashboard');
  step('instructor dashboard');

  // Create a course (no instructor select)
  const course = `ICourse${stamp}`;
  await p.goto(base + '/instructor/courses/new');
  await p.waitForSelector('#courseName');
  if (await p.locator('#instructorId').count()) throw new Error('instructor select should be hidden');
  await p.fill('#courseName', course); await p.fill('#description', 'Instructor course');
  await p.setInputFiles('#imgCover', png);
  await p.fill('#price', '99'); await p.fill('#credits', '2');
  await p.fill('#demoVideoUrl', 'https://example.com/demo.mp4');
  await p.fill('#startDate', '2025-03-01'); await p.fill('#endDate', '2025-07-01');
  await p.selectOption('#departmentId', { index: 1 });
  await p.click('button:has-text("Create")');
  await p.waitForURL('**/instructor/courses');
  await p.waitForSelector(`td:text("${course}")`);
  step('course created by instructor');
  await noSwal();

  // Add-Process (section) from the list
  await p.locator('tr', { hasText: course }).locator('button:has-text("Add-Process")').click();
  await p.fill('#fm-title', 'Week 1'); await p.fill('#fm-content', 'Getting started');
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector('text=Curriculum added');
  await noSwal();
  step('section added (Add-Process)');

  // Curriculum manager
  await p.locator('tr', { hasText: course }).locator('a:has-text("View")').click();
  await p.waitForSelector('h5:text("Week 1")');

  await p.click('button:has-text("Add Reference")');
  await p.fill('#fm-resourceTitle', 'Angular docs'); await p.fill('#fm-resourceURL', 'https://angular.dev');
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector('a:has-text("Angular docs")');
  await noSwal();
  step('reference added');

  await p.click('button:has-text("Add Video")');
  await p.fill('#fm-title', 'Intro video');
  await p.setInputFiles('#fm-video', { name: 'intro.mp4', mimeType: 'video/mp4', buffer: Buffer.from([0, 0, 0, 24, 102, 116, 121, 112]) });
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector('a:has-text("Intro video")');
  await noSwal();
  step('video uploaded');

  // Second plan: edit a video (title only; the file is kept)
  await p.click('button[title="Edit video"]');
  await p.waitForFunction(() => document.querySelector('#fm-title')?.value === 'Intro video');
  await p.fill('#fm-title', 'Intro video v2');
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector('a:has-text("Intro video v2")');
  await noSwal();
  step('video title edited');

  await p.click('button:has-text("Add Assignment")');
  await p.fill('#fm-title', 'HW 1');
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector('.modal-body >> text=This field is required.');
  await p.setInputFiles('#fm-content', { name: 'hw1.pdf', mimeType: 'application/pdf', buffer: Buffer.from('%PDF-1.4') });
  await p.fill('#fm-deadLine', '2025-05-01T23:59');
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector('a:has-text("HW 1")');
  await noSwal();
  step('assignment added (file required)');

  await p.click('button:has-text("Add Quiz")');
  await p.fill('#fm-title', 'Quiz 1'); await p.fill('#fm-examDate', '2025-04-01T10:00');
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector('td:text("Quiz 1")');
  await noSwal();
  await p.screenshot({ path: `${out}/p5-curriculum.png`, fullPage: true });
  step('quiz added');

  // Questions of the quiz
  await p.locator('tr', { hasText: 'Quiz 1' }).locator('a:has-text("Questions")').click();
  await p.waitForSelector('text=Questions for Quiz 1');
  await p.click('text=Create New Question');
  await p.fill('#questionText', 'What is Angular?');
  const choices = p.locator('input[formcontrolname=choiceText]');
  await choices.nth(0).fill('A framework'); await choices.nth(1).fill('A database');
  await p.click('button:has-text("Save")');
  await p.waitForSelector('text=Please mark at least one choice as correct.');
  step('question needs a correct answer');
  await p.check('#correct-0');
  await p.click('button:has-text("Save")');
  await p.waitForSelector('h5:text("What is Angular?")');
  await p.waitForSelector('.list-group-item-success:has-text("A framework")');
  step('question with choices created');
  await noSwal();

  // Exams list
  await p.goto(base + '/instructor/exams');
  await p.waitForSelector('td:text("Quiz 1")');
  const qCount = await p.locator('tr', { hasText: 'Quiz 1' }).locator('td').nth(3).innerText();
  if (qCount.trim() !== '1') throw new Error('expected 1 question, got ' + qCount);
  step('exams list shows the quiz with 1 question');

  // Assignments page
  await p.goto(base + '/instructor/assignments');
  await p.waitForSelector('text=Please select a Course.');
  await p.selectOption('#courseId', { label: course });
  await p.waitForSelector('text=HW 1 (Due:');
  await p.click('a:has-text("Submitions")');
  await p.waitForSelector('text=No found any submitions');
  step('assignments page + submissions modal');
  await p.keyboard.press('Escape');
  await p.waitForSelector('.modal', { state: 'detached' });

  // Second plan: reorder and delete curriculum sections
  await p.goto(base + '/instructor/courses');
  await p.locator('tr', { hasText: course }).locator('a:has-text("View")').click();
  await p.waitForSelector('h5:text("Week 1")');
  await p.click('button:has-text("Add-Process")');
  await p.fill('#fm-title', 'Week 2'); await p.fill('#fm-content', 'Next steps');
  await p.click('.modal-footer button[type=submit]');
  await p.waitForSelector('h5:text("Week 2")');
  await noSwal();
  const sectionTitles = () => p.locator('.list-group > li h5').allInnerTexts();
  await p.locator('li', { has: p.locator('h5:text("Week 2")') }).locator('button[title="Move up"]').click();
  await p.waitForFunction(() => document.querySelector('.list-group > li h5')?.textContent === 'Week 2');
  step('section moved up: ' + (await sectionTitles()).join(' > '));

  await p.locator('li', { has: p.locator('h5:text("Week 1")') }).locator('button[title="Delete section"]').click();
  await p.waitForSelector('.swal2-container:has-text("1 video(s), 1 reference(s), 1 assignment(s) and 1 quiz(zes)")');
  await p.click('.swal2-cancel');
  await noSwal();
  step('delete confirmation lists what will be removed (cancelled)');
  await p.locator('li', { has: p.locator('h5:text("Week 2")') }).locator('button[title="Delete section"]').click();
  await p.click('.swal2-confirm');
  await p.waitForSelector('h5:text("Week 2")', { state: 'detached' });
  await noSwal();
  step('empty section deleted: ' + (await sectionTitles()).join(' > '));

  console.log('console errors:', errors.filter((e) => !/status of (400|401|403|404)/.test(e)));
  await b.close();
})().catch((e) => { console.error('FAILED', e.message); process.exit(1); });
