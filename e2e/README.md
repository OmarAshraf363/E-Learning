# سيناريوهات E2E (Playwright)

7 سيناريوهات بتشتغل على الـ API الحقيقية والـ Angular الحقيقية، واحد لكل منطقة اتنقلت من الـ MVC.
اتعمل بيها التحقق من كل Phase، وبتستخدمها كـ Regression قبل أي تعديل كبير.

| الملف | بيغطي |
|---|---|
| `01-auth-and-users.js` | الـ Login، والـ Register، والـ Lock/Unlock، والبروفايل، والـ Refresh token |
| `02-admin.js` | الأقسام، والتراكات، والكورسات، والسنين، والجداول، والتسجيل |
| `03-public-catalog.js` | الـ Home، والبحث، والفلاتر، وتفاصيل الكورس، والـ Category، والـ Topic، و Teach on BFCAI |
| `04-cart-and-checkout.js` | السلة، والـ Badge، والـ Checkout، وصفحات النجاح والإلغاء |
| `05-instructor.js` | كورسات الـ Instructor، والمنهج، والفيديو، والواجبات، والكويزات والأسئلة |
| `06-student.js` | الداشبورد، والـ Player، والـ Quiz بعد 25% من الفيديو، والتسليم، والفيدباك |
| `07-community.js` | البوستات، والبحث، والـ Reactions، والكومنتات والردود، والتعديل والحذف |

## التشغيل
```bash
# 1) الخدمات (من فولدر backend و frontend)
docker compose up -d
dotnet run --project src/BFCAI.Api --launch-profile http
npm start

# 2) السيناريوهات
cd e2e
npm install
npx playwright install chromium
npm test                       # كلهم بالترتيب
node 05-instructor.js          # سيناريو واحد
```

- كل سيناريو بيعمل داتا جديدة باسم فريد، فينفع تشغّله أكتر من مرة على نفس الداتابيز.
- الـ Screenshots بتتحفظ في `e2e/screenshots/`، والفولدر ده متجاهَل في Git.
- عشان تغيّر العناوين: `BASE_URL=http://... API_URL=http://.../api npm test`
- `fixtures/lesson.webm` فيديو 8 ثواني متولّد بـ ffmpeg، وبيستخدمه اختبار الـ Quiz اللي بيظهر بعد 25% من الفيديو.
