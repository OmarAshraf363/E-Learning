# BFCAI E-Learning

منصة تعليم إلكتروني لكلية الحاسبات والذكاء الاصطناعي بجامعة بنها.

| الجزء | الفولدر | الدليل |
|---|---|---|
| API (.NET 10، Clean Architecture) | `backend/` | [backend/README.md](backend/README.md) |
| Angular 22 | `frontend/` | [frontend/README.md](frontend/README.md) |
| سيناريوهات E2E | `e2e/` | [e2e/README.md](e2e/README.md) |
| نبذة عن المشروع | — | [PROJECT_BRIEF.md](PROJECT_BRIEF.md) |
| خطة النقل وحالتها والـ Backlog | `docs/` | [docs/MIGRATION_PLAN.md](docs/MIGRATION_PLAN.md) |
| مقارنة الـ MVC بالنسخة الجديدة | `docs/` | [docs/PARITY_CHECKLIST.md](docs/PARITY_CHECKLIST.md) |

## تشغيل سريع
```bash
cd backend && docker compose up -d
dotnet ef database update --project src/BFCAI.Infrastructure --startup-project src/BFCAI.Api
dotnet run --project src/BFCAI.Api --launch-profile http     # http://localhost:5151/scalar
cd ../frontend && npm install && npm start                    # http://localhost:4200
```

> كود الـ MVC القديم (`Banha UniverCity/` وباقي المشاريع القديمة) لسه موجود كمرجع، ومش هيتشال غير بعد الموافقة.
