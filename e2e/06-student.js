const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');
const out = process.argv[2] || path.join(__dirname, 'screenshots');
require('fs').mkdirSync(out, { recursive: true });
const api = process.env.API_URL || 'http://localhost:5151/api';
const base = process.env.BASE_URL || 'http://localhost:4200';

async function call(method, path, token, body, isForm) {
  const res = await fetch(api + path, {
    method,
    headers: { ...(token ? { Authorization: 'Bearer ' + token } : {}), ...(body && !isForm ? { 'Content-Type': 'application/json' } : {}) },
    body: body ? (isForm ? body : JSON.stringify(body)) : undefined,
  });
  if (!res.ok) throw new Error(`${method} ${path} -> ${res.status} ${await res.text()}`);
  return res.status === 204 ? null : res.json();
}
const login = async (email, password) => (await call('POST', '/auth/login', null, { email, password })).accessToken;

(async () => {
  const stamp = Date.now().toString().slice(-6);
  // ---- data setup through the API
  const admin = await login('Admin@gmail.com', 'Admin@123');
  const instEmail = `pi${stamp}@test.com`, stuEmail = `ps${stamp}@test.com`;
  await call('POST', '/admin/users', admin, { userName: instEmail, email: instEmail, role: 'Instructor', fullName: 'Prof ' + stamp, password: 'Pass@123' });
  await call('POST', '/admin/users', admin, { userName: stuEmail, email: stuEmail, role: 'Student', fullName: 'Student ' + stamp, password: 'Pass@123' });
  const inst = await login(instEmail, 'Pass@123');

  const cf = new FormData();
  Object.entries({ CourseName: 'Player ' + stamp, Description: 'Course for the player', Credits: '3', Price: '10',
    DemoVideoUrl: 'https://e.com/v', StartDate: '2025-01-01', EndDate: '2025-12-01', DepartmentId: '1' }).forEach(([k, v]) => cf.append(k, v));
  cf.append('ImgCover', new Blob([Buffer.from([1])]), 'c.png');
  const course = await call('POST', '/instructor/courses', inst, cf, true);
  const section = await call('POST', `/instructor/courses/${course.id}/sections`, inst, { title: 'Week 1', content: 'First steps' });
  const vf = new FormData(); vf.append('Title', 'Lesson video'); vf.append('Video', new Blob([fs.readFileSync(path.join(__dirname, 'fixtures', 'lesson.webm'))]), 'lesson.webm');
  await call('POST', `/instructor/sections/${section.id}/videos`, inst, vf, true);
  await call('POST', `/instructor/sections/${section.id}/references`, inst, { resourceTitle: 'MDN', resourceURL: 'https://developer.mozilla.org' });
  const af = new FormData(); af.append('Title', 'Homework 1'); af.append('DeadLine', '2025-06-01'); af.append('Content', new Blob([Buffer.from('%PDF')]), 'hw.pdf');
  await call('POST', `/instructor/sections/${section.id}/assignments`, inst, af, true);
  const quiz = await call('POST', `/instructor/sections/${section.id}/quizzes`, inst, { title: 'Quick quiz', examDate: '2025-03-01T10:00' });
  await call('POST', `/instructor/exams/${quiz.id}/questions`, inst, { questionText: 'Is Angular a framework?', choices: [{ choiceText: 'Yes', isCorrect: true }, { choiceText: 'No', isCorrect: false }] });
  await call('POST', '/admin/enrollments/by-email', admin, { email: stuEmail, courseId: course.id });
  console.log('✔ data prepared through the API (course', course.id + ')');

  // ---- student UI
  const b = await chromium.launch();
  const p = await b.newPage({ viewport: { width: 1366, height: 900 } });
  const errors = [];
  p.on('console', (m) => m.type() === 'error' && errors.push(m.text()));
  p.on('pageerror', (e) => errors.push('PAGEERROR ' + e.message));
  const step = (s) => console.log('✔', s);
  const noSwal = () => p.waitForSelector('.swal2-container', { state: 'detached', timeout: 10000 });

  await p.goto(base + '/auth/login');
  await p.fill('#email', stuEmail); await p.fill('#password', 'Pass@123'); await p.click('#login-submit');
  await p.waitForURL(base + '/');
  await p.goto(base + '/student');
  await p.waitForSelector(`td:text("Player ${stamp}")`);
  await p.waitForTimeout(500);
  await p.screenshot({ path: `${out}/p6-dashboard.png` });
  step('student dashboard lists the course');

  await p.locator('tr', { hasText: 'Player ' + stamp }).locator('button[title="Quick access"]').click();
  await p.waitForSelector('.modal .resource-card:has-text("Lesson video")');
  await p.click('.modal-footer button:has-text("Close")');
  step('quick access modal');

  await p.locator('tr', { hasText: 'Player ' + stamp }).locator('a:has-text("Material")').click();
  await p.waitForSelector('text=Course Materials');
  await p.click('.part:has-text("Week 1")');
  await p.waitForSelector('h1.course-title:text("Week 1")');
  step('course player: section content');

  // video -> quiz after 25%
  await p.selectOption('app-section-content select', { label: 'Lesson video' });
  await p.waitForSelector('video');
  await p.evaluate(() => { const v = document.querySelector('video'); v.muted = true; return v.play(); });
  await p.waitForSelector('text=Is Angular a framework?', { timeout: 20000 });
  step('quiz popped up after 25% of the video');
  await p.click('.modal button:has-text("Finish")');

  // assignment submission
  await p.fill('input[aria-label="Solution for Homework 1"]', 'https://github.com/me/hw1');
  await p.click('button:has-text("Submit")');
  await p.waitForSelector('.submitted-badge');
  step('assignment submitted -> "Submitted"');
  await noSwal();

  // course feedback with rating
  await p.click('text=Create New Feedback');
  await p.click('label[for="star-4"]');
  await p.fill('#content', 'Great course');
  await p.click('.modal-body button[type=submit]');
  await p.waitForSelector('.feedback-text:text("Great course")');
  await noSwal();
  step('course feedback with rating added');
  await p.screenshot({ path: `${out}/p6-player.png`, fullPage: true });

  const details = await call('GET', `/catalog/courses/${course.id}`);
  if (details.rate !== 4) throw new Error('course rate not recalculated: ' + details.rate);
  step('course rate recalculated to 4');

  // my courses
  await p.goto(base + '/student/courses');
  await p.waitForSelector(`h5:text("Player ${stamp}")`);
  step('my courses page');

  console.log('console errors:', errors.filter((e) => !/status of (400|401|403|404)/.test(e)));
  await b.close();
})().catch((e) => { console.error('FAILED', e.message); process.exit(1); });
