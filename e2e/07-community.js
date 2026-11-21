const { chromium } = require('playwright');
const path = require('path');
const out = process.argv[2] || path.join(__dirname, 'screenshots');
require('fs').mkdirSync(out, { recursive: true });
const base = process.env.BASE_URL || 'http://localhost:4200';
const png = { name: 'post.png', mimeType: 'image/png', buffer: Buffer.from('iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVR42mNk+M9QDwADhgGAWjR9awAAAABJRU5ErkJggg==', 'base64') };
(async () => {
  const b = await chromium.launch();
  const p = await b.newPage({ viewport: { width: 1366, height: 900 } });
  const errors = [];
  p.on('console', (m) => m.type() === 'error' && errors.push(m.text()));
  p.on('pageerror', (e) => errors.push('PAGEERROR ' + e.message));
  const step = (s) => console.log('✔', s);
  const stamp = Date.now().toString().slice(-6);

  // register + open community
  await p.goto(base + '/auth/register');
  await p.fill('#fullName', 'Poster ' + stamp); await p.fill('#email', `poster${stamp}@test.com`);
  await p.fill('#password', 'Pass@123'); await p.fill('#confirmPassword', 'Pass@123');
  await p.click('#registerSubmit');
  await p.waitForURL(base + '/');
  await p.goto(base + '/community');
  await p.waitForSelector('text=Top instructors');
  step('community feed (logged in)');

  // community filter
  await p.click('a:has-text("Web Development Community")');
  await p.waitForURL('**/community?id=1');
  await p.waitForSelector('h5:has-text("Web Development Community |")');
  step('community selected');

  // create post with keyword + image
  const tag = 'tag' + stamp;
  await p.click('.content-header-newPost input');
  await p.fill('#post-content', 'My first post ' + stamp);
  await p.click('button:has-text("Add Key Word")');
  await p.fill('input[aria-label="Key word 1"]', tag);
  await p.setInputFiles('#post-image', png);
  await p.waitForSelector('img[alt="your image"]');
  await p.click('button:has-text("Create Post")');
  const post = p.locator('.post', { hasText: 'My first post ' + stamp });
  await post.waitFor();
  if (!(await post.locator('.post-image img').count())) throw new Error('image not shown');
  step('post created with keyword and image');

  // search by keyword
  await p.fill('input[name=word]', tag);
  await p.click('button[aria-label="Search posts"]');
  await p.waitForURL(`**word=${tag}**`);
  await p.waitForTimeout(500);
  if ((await p.locator('.post').count()) !== 1) throw new Error('keyword search should return 1 post');
  step('keyword search');

  // react + comment + reply
  await post.locator('.post-controls span:has-text("React")').click();
  await post.locator('text=Reacted').waitFor();
  await post.locator('span:has-text("1 Reactions")').waitFor();
  await post.locator('input[aria-label^="Comment on post"]').fill('Great post!');
  await post.locator('button[aria-label="Send comment"]').click();
  await post.locator('text=1 Comment').waitFor();
  step('reaction toggle + inline comment');

  await post.locator('text=1 Comment').click();
  await p.waitForSelector('.modal .comment-content:has-text("Great post!")');
  await p.click('.modal .react-btn');
  await p.click('.modal button:has-text("Reply")');
  await p.fill('.modal textarea', 'Thanks!');
  await p.click('.modal button:has-text("Post Reply")');
  await p.waitForSelector('.modal span:has-text("1 Replies")');
  await p.click('.modal span:has-text("1 Replies")');
  await p.waitForSelector('.modal .comment-content:has-text("Thanks!")');
  step('comments modal: love reaction + reply + replies modal');
  await p.screenshot({ path: `${out}/p7-comments.png` });
  while (await p.locator('ngb-modal-window').count()) { await p.keyboard.press('Escape'); await p.waitForTimeout(400); }

  await post.locator('.post-reaction span:has-text("Reactions")').click();
  await p.waitForSelector(`.modal .user-name:has-text("Poster ${stamp}")`);
  while (await p.locator('ngb-modal-window').count()) { await p.keyboard.press('Escape'); await p.waitForTimeout(400); }
  step('who reacted modal');

  // edit post (keywords replaced, image kept)
  await p.waitForTimeout(400);
  await post.locator('.fa-bars').click();
  await post.locator('button:has-text("Edit")').click();
  await p.waitForFunction(() => document.querySelector('#post-content')?.value?.startsWith('My first post'));
  await p.fill('#post-content', 'Edited post ' + stamp);
  await p.fill('input[aria-label="Key word 1"]', 'edited' + stamp);
  await p.click('button:has-text("Update Post")');
  const edited = p.locator('.post', { hasText: 'Edited post ' + stamp });
  await p.goto(base + `/community?word=edited${stamp}`);
  await edited.waitFor();
  if (!(await edited.locator('.post-image img').count())) throw new Error('image lost after edit');
  step('post edited: new keyword searchable, image kept');
  await p.screenshot({ path: `${out}/p7-feed.png` });

  // delete (has comments, replies, reactions)
  await edited.locator('.fa-bars').click();
  await edited.locator('button:has-text("Delete")').click();
  await p.click('.swal2-confirm');
  await p.waitForSelector('text=Not found posts');
  step('post with comments/replies/reactions deleted');

  // top instructor -> user filter
  await p.goto(base + '/community');
  await p.locator('.instructor-max-enroll a').first().click();
  await p.waitForURL('**userId=**');
  await p.waitForSelector('.target-user');
  step('top instructor -> user posts');

  console.log('console errors:', errors.filter((e) => !/status of (400|401|403|404)/.test(e)));
  await b.close();
})().catch((e) => { console.error('FAILED', e.message); process.exit(1); });
