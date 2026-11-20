# BFCAI API (.NET 10, Clean Architecture)

API جديدة بتحلّ محل مشروع الـ MVC (`Banha UniverCity/`) وبنفس اللوجيك. الفرونت بتاعها موجود في `../frontend`.

## التشغيل محليًا

```bash
# 1) SQL Server + Redis
docker compose up -d

# 2) إنشاء/تحديث الداتابيز (BFCAI)
dotnet ef database update --project src/BFCAI.Infrastructure --startup-project src/BFCAI.Api

# 3) تشغيل الـ API  ->  http://localhost:5151   (Scalar UI: http://localhost:5151/scalar)
dotnet run --project src/BFCAI.Api --launch-profile http

# Tests
dotnet test
```

| المتطلب | الإصدار |
|---|---|
| .NET SDK | 10.0 |
| dotnet-ef | `dotnet tool install -g dotnet-ef --version 10.0.15` |
| Docker | لـ SQL Server و Redis |

**الأدمن الافتراضي (من الـ Seed):** `Admin@gmail.com` / `Admin@123`

## الإعدادات والأسرار

`appsettings.Development.json` فيه قيم تطوير محلية بس (باسورد SQL الخاص بـ docker-compose ومفتاح JWT للتطوير).
أي سر حقيقي **ممنوع يتكتب في الريبو**؛ استخدم `user-secrets` أو Environment Variables:

```bash
cd src/BFCAI.Api
dotnet user-secrets init
dotnet user-secrets set "Stripe:SecretKey" "sk_test_..."
dotnet user-secrets set "Jwt:Key" "<random string of 32+ chars>"
```

في الـ Production بتتحط كـ Environment Variables: `Stripe__SecretKey`, `Jwt__Key`, `ConnectionStrings__DefaultConnection`, `ConnectionStrings__Redis`.

## استخدام داتابيز موجودة من مشروع الـ MVC

الـ Schema متطابقة 100% مع الـ MVC (اتأكدنا بمقارنة الجداول والأعمدة والـ FKs والـ Indexes). شغّل السكربت ده **مرة واحدة** على الداتابيز القديمة:

```bash
sqlcmd -S <server> -d <db> -I -i docs/database/attach-existing-database.sql
dotnet ef database update --project src/BFCAI.Infrastructure --startup-project src/BFCAI.Api
```

ده بيضيف جدول `RefreshTokens` بس، والداتا كلها بتفضل زي ما هي.

**الملفات المرفوعة:** انسخ فولدرات `Covers`, `videos`, `assignments`, `uploads` من `Banha UniverCity/wwwroot` لـ `src/BFCAI.Api/wwwroot`، أو غيّر `Storage:RootPath` بحيث يشاور على المكان بتاعها.

## اقرأ كمان

- [docs/ARCHITECTURE.md](docs/ARCHITECTURE.md): الطبقات وقواعدها.
- [docs/ADD-A-FEATURE.md](docs/ADD-A-FEATURE.md): خطوات إضافة Feature جديدة.
