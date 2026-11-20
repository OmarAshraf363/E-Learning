import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NotifyService } from '../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../shared/forms.util';
import { matchValidator } from '../../../shared/validators';
import { AccountApi } from '../data-access/account.api';

/** Legacy Identity/Account/Manage/ChangePassword. */
@Component({
  selector: 'app-change-password-page',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  template: `
    <h3>Change password</h3>
    <div class="row">
      <div class="col-md-6">
        <form [formGroup]="form" (ngSubmit)="submit()" novalidate>
          @for (message of generalErrors(); track message) {
            <div class="text-danger">{{ message }}</div>
          }
          <div class="form-floating mb-3">
            <input id="oldPassword" type="password" formControlName="oldPassword" class="form-control" autocomplete="current-password" placeholder="Current password" />
            <label for="oldPassword" class="form-label">Current password</label>
            <app-form-errors [control]="form.controls.oldPassword" />
          </div>
          <div class="form-floating mb-3">
            <input id="newPassword" type="password" formControlName="newPassword" class="form-control" autocomplete="new-password" placeholder="New password" />
            <label for="newPassword" class="form-label">New password</label>
            <app-form-errors [control]="form.controls.newPassword" />
          </div>
          <div class="form-floating mb-3">
            <input id="confirmPassword" type="password" formControlName="confirmPassword" class="form-control" autocomplete="new-password" placeholder="Confirm new password" />
            <label for="confirmPassword" class="form-label">Confirm new password</label>
            <app-form-errors [control]="form.controls.confirmPassword" />
          </div>
          <button type="submit" class="w-100 btn btn-lg btn-primary" [disabled]="saving()">Update password</button>
        </form>
      </div>
    </div>
  `,
})
export class ChangePasswordPageComponent {
  private readonly api = inject(AccountApi);
  private readonly notify = inject(NotifyService);

  readonly form = inject(FormBuilder).nonNullable.group(
    {
      oldPassword: ['', Validators.required],
      newPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      confirmPassword: [''],
    },
    { validators: matchValidator('newPassword', 'confirmPassword', 'The new password and confirmation password do not match.') },
  );
  readonly saving = signal(false);
  readonly generalErrors = signal<string[]>([]);

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    this.api.changePassword(this.form.getRawValue()).subscribe({
      next: () => {
        this.saving.set(false);
        this.form.reset();
        this.notify.success('Your password has been changed.');
      },
      error: (err: unknown) => {
        this.saving.set(false);
        this.generalErrors.set(applyServerErrors(this.form, err));
      },
    });
  }
}
