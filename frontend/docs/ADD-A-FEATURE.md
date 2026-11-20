# إضافة Feature / صفحة جديدة

المثال: صفحة الأقسام في الأدمن. الـ API بتاعها `api/admin/departments`.

## 1) الـ Model: `features/admin/departments/models/department.ts`
```ts
export interface Department { id: number; name: string; description: string; cover?: string | null; }
export interface DepartmentRequest { departmentName: string; departmentDescription: string; cover?: File | null; }
```

## 2) الـ Api service: `features/admin/departments/data-access/departments.api.ts`
بيورث كل عمليات الـ CRUD من `BaseApiService`.
```ts
@Injectable({ providedIn: 'root' })
export class DepartmentsApi extends BaseApiService<Department, DepartmentRequest> {
  protected readonly resource = 'admin/departments';
  protected override readonly useFormData = true;   // لأن فيه رفع صورة

  // أي Endpoint إضافي:
  courses(id: number) { return this.api.get<Course[]>(`${this.resource}/${id}/courses`); }
}
```

## 3) الـ Store: `features/admin/departments/data-access/departments.store.ts`
```ts
@Injectable({ providedIn: 'root' })
export class DepartmentsStore extends CrudStore<Department, DepartmentRequest> {
  constructor() { super(inject(DepartmentsApi), (d) => d.id); }
}
```
لو الـ Feature مش CRUD، اعمل Service عادي فيه `signal()`/`computed()` وبينادي الـ Api.

## 4) الصفحة: `features/admin/departments/pages/departments-page.component.ts`
```ts
@Component({
  selector: 'app-departments-page',
  imports: [RouterLink],
  template: `
    @for (d of store.items(); track d.id) {
      <tr><td>{{ d.name }}</td><td><button (click)="remove(d.id)">Delete</button></td></tr>
    } @empty { <tr><td>No departments</td></tr> }
  `,
})
export class DepartmentsPageComponent {
  readonly store = inject(DepartmentsStore);
  private readonly notify = inject(NotifyService);

  constructor() { void this.store.load(); }

  async remove(id: number) {
    if (await this.notify.confirm()) { await this.store.remove(id); this.notify.success('Deleted'); }
  }
}
```

### الفورمز
- استخدم Typed Reactive Forms (`FormBuilder.nonNullable`).
- لو الـ Save فشل: `applyServerErrors(form, error)`، والأخطاء بتظهر في `<app-form-errors [control]="form.controls.x" />`.

### الـ Modals
- **فورم صغير** (حقلين أو تلاتة أو رفع ملف): استخدم `openFormModal` من `shared/components/form-modal.component.ts`. بتديله الحقول والـ `submit`، وهو بيتولّى الـ Validation وأخطاء الـ API:
  ```ts
  const saved = await openFormModal<{ title: string; video: File | null }>(this.modal, {
    title: 'Add Video',
    fields: [
      { name: 'title', label: 'Title', required: true },
      { name: 'video', label: 'Video', type: 'file', accept: 'video/*', required: true },
    ],
    submit: (v) => firstValueFrom(this.api.uploadVideo(sectionId, v)),
  });
  if (saved !== undefined) this.load();
  ```
- **محتوى عرض بسيط** (تفاصيل أو جدول): `<ng-template #x let-modal>` في نفس الصفحة، وبعدين `modal.open(this.x())`.
- **فورم كبير أو منطق كتير:** Component مستقل بـ `NgbActiveModal`، زي `DepartmentFormModalComponent`.

## 5) الـ Route: `features/admin/admin.routes.ts`
```ts
{ path: 'departments', loadComponent: () => import('./departments/pages/departments-page.component').then(m => m.DepartmentsPageComponent) },
```
الـ Route params والـ Query params بتوصل كـ `input()` لوحدها (`withComponentInputBinding`).

## Checklist
- [ ] Model و Api و Store جوه `features/<area>/<feature>/`
- [ ] الصفحة في `pages/`، وأي جزء بيتكرر في `components/`
- [ ] الـ Route في `<area>.routes.ts`، والصلاحيات متعرفة على الـ Layout route في `app.routes.ts`
- [ ] الـ Styles: Bootstrap و Classes الـ Layout القديمة أولًا، وبعدها `styles` جوه الـ Component
