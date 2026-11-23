import { Component, inject, input, OnInit, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NotifyService } from '../../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../../shared/forms.util';
import { LookupsApi } from '../../shared/lookups.api';
import { UsersApi } from '../data-access/users.api';
import { UsersStore } from '../data-access/users.store';

/** Legacy Admin/User/Upsert (+ the admin variant of the Register page). Route: users/new | users/:id */
@Component({
  selector: 'app-user-upsert-page',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  templateUrl: './user-upsert-page.component.html',
  styles: `
    form input, form select { background: #3d3d3d; color: #eee; }
  `,
})
export class UserUpsertPageComponent implements OnInit {
  private readonly api = inject(UsersApi);
  private readonly store = inject(UsersStore);
  private readonly router = inject(Router);
  private readonly notify = inject(NotifyService);

  /** Route param (undefined on "new"). */
  readonly id = input<string>();
  /** ?role= pre-selects the role when coming from the Students / Instructors list. */
  readonly role = input<string>();

  readonly roles = signal<string[]>([]);
  readonly saving = signal(false);
  readonly generalErrors = signal<string[]>([]);

  readonly form = inject(FormBuilder).nonNullable.group({
    fullName: [''],
    userName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    password: [''],
    role: ['', Validators.required],
    departmentId: new FormControl<number | null>(null),
  });

  /** The department is shown for instructors only (without one they can pick any department). */
  readonly selectedRole = toSignal(this.form.controls.role.valueChanges, { initialValue: '' });
  readonly departments = toSignal(inject(LookupsApi).departments(), { initialValue: [] });

  get isEdit(): boolean {
    return !!this.id();
  }

  ngOnInit(): void {
    this.api.roles().subscribe((roles) => this.roles.set(roles));

    const id = this.id();
    if (id) {
      this.api.getById(id).subscribe((user) => this.form.patchValue(user));
    } else {
      this.form.controls.password.addValidators(Validators.required);
      if (this.role()) this.form.patchValue({ role: this.role() });
    }
  }

  async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    this.generalErrors.set([]);

    const value = this.form.getRawValue();
    try {
      await this.store.save(
        {
          ...value,
          fullName: value.fullName || null,
          password: this.isEdit ? null : value.password,
          departmentId: value.role === 'Instructor' ? value.departmentId : null,
        },
        this.id(),
      );
      this.notify.success(this.isEdit ? 'User updated.' : 'Added New User successfully');
      void this.router.navigate(['/admin/users'], { queryParams: { filter: value.role } });
    } catch (err) {
      this.generalErrors.set(applyServerErrors(this.form, err));
    } finally {
      this.saving.set(false);
    }
  }
}
