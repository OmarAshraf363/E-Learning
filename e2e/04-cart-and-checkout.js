const { chromium } = require('playwright');
const path = require('path');
const out = process.argv[2] || path.join(__dirname, 'screenshots');
require('fs').mkdirSync(out, { recursive: true });
const base = process.env.BASE_URL || 'http://localhost:4200';
(async () => {
  const b = await chromium.launch();
  const p = await b.newPage({ viewport: { width: 1366, height: 900 } });
  const errors = [];
  p.on('console', (m) => m.type() === 'error' && errors.push(m.text()));
  p.on('pageerror', (e) => errors.push('PAGEERROR ' + e.message));
  const step = (s) => console.log('✔', s);

  // Register a student first (then log out)
  const email = `buyer${Date.now()}@test.com`;
  await p.goto(base + '/auth/register');
  await p.fill('#fullName', 'Buyer'); await p.fill('#email', email);
  await p.fill('#password', 'Pass@123'); await p.fill('#confirmPassword', 'Pass@123');
  await p.click('#registerSubmit');
  await p.waitForURL(base + '/');
  await p.click('button[title=Logout]');
  await p.waitForURL('**/auth/login');

  // Anonymous "Add to Cart" -> login -> back to cart with the course added
  await p.goto(base + '/courses/2');
  await p.waitForSelector('text=About the Instructor');
  await p.click('a:has-text("Enroll")');
  await p.waitForURL('**/auth/login?returnUrl=**');
  step('anonymous add-to-cart asks for login');
  await p.fill('#email', email); await p.fill('#password', 'Pass@123'); await p.click('#login-submit');
  await p.waitForURL(/\/cart$/);
  await p.waitForSelector('h5.card-title:text("Digital Circuits")');
  await p.waitForSelector('.cart-counter:text("1")');
  step('after login the course is in the cart, badge = 1');

  // Second plan: a course can be in the cart only once
  await p.goto(base + '/cart?add=2');
  await p.waitForSelector('.swal2-container:has-text("This course is already in your cart.")');
  await p.click('.swal2-confirm');
  await p.waitForSelector('h5.card-title:text("Digital Circuits")');
  if ((await p.locator('.col-lg-8 h5.card-title').count()) !== 1) throw new Error('the course must be in the cart once');
  if (await p.locator('text=Tax').count()) throw new Error('the tax line should be removed');
  step('same course twice -> "already in your cart", no tax line');

  await p.goto(base + '/courses/2');
  await p.waitForSelector('.enroll-btn a:text("Go to Cart")');
  step('course details: "Go to Cart" for a course in the cart');

  // The admin enrolls the buyer in course 1 (API) -> "Open course", and it can't be added to the cart
  const api = process.env.API_URL || 'http://localhost:5151/api';
  const login = await (await p.request.post(api + '/auth/login', { data: { email: 'Admin@gmail.com', password: 'Admin@123' } })).json();
  const enroll = await p.request.post(api + '/admin/enrollments/by-email', {
    data: { email, courseId: 1 }, headers: { Authorization: `Bearer ${login.accessToken}` },
  });
  if (!enroll.ok()) throw new Error('enroll failed ' + enroll.status());
  await p.goto(base + '/courses/1');
  await p.reload(); // reload the cart summary (enrolled courses) after the admin's change
  await p.waitForSelector('.enroll-btn a:text("Open course")');
  await p.click('.enroll-btn a:text("Open course")');
  await p.waitForURL('**/student/courses/1');
  step('enrolled course: "Open course" opens the course player');
  await p.goto(base + '/cart?add=1');
  await p.waitForSelector('.swal2-container:has-text("You are already enrolled in this course.")');
  await p.click('.swal2-confirm');
  step('enrolled course can\'t be added to the cart');

  // Add another course from a card popover
  await p.goto(base + '/courses');
  await p.waitForSelector('.one-course');
  const card = p.locator('.one-course').filter({ has: p.locator('a:has-text("Add to Cart")') }).first();
  const added = (await card.locator('.course-title h5').innerText()).trim();
  await card.hover();
  await card.locator('a:has-text("Add to Cart")').click();
  await p.waitForURL(/\/cart$/);
  await p.waitForSelector('.cart-counter:text("2")');
  await p.waitForTimeout(500);
  await p.screenshot({ path: `${out}/p4-cart.png` });
  step(`second course added from the card popover (${added}), badge = 2`);

  // Remove one
  await p.locator('.card', { hasText: added }).locator('button[title=Remove]').click();
  await p.waitForSelector('.cart-counter:text("1")');
  step('remove item');

  // Checkout: Stripe key not configured in this environment -> clear message, no crash
  await p.click('text=Proceed to Checkout');
  await p.waitForSelector('.alert-danger:text("Online payment is not available right now.")');
  step('checkout without Stripe key -> clear error message');

  // Success page with a fake / unpaid session must NOT enroll
  await p.goto(base + '/cart/checkout/success?session_id=cs_test_fake');
  await p.waitForSelector('text=Payment not confirmed');
  step('success page with unpaid session -> not confirmed (no enrollment)');

  await p.goto(base + '/cart/checkout/cancel');
  await p.waitForSelector('text=Payment Canceled');
  step('cancel page');

  console.log('console errors:', errors.filter((e) => !/status of (400|401|404)/.test(e)));
  await b.close();
})().catch((e) => { console.error('FAILED', e.message); process.exit(1); });
