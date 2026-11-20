import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { FormErrorsComponent } from '../../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../../shared/forms.util';
import { matchValidator } from '../../../../shared/validators';

/** Legacy Identity/Account/Register page (visitor -> Student account). */
@Component({
  selector: 'app-register-page',
  imports: [ReactiveFormsModule, RouterLink, FormErrorsComponent],
  templateUrl: './register-page.component.html',
  styleUrl: './register-page.component.scss',
})
export class RegisterPageComponent {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  readonly form = inject(FormBuilder).nonNullable.group(
    {
      fullName: [''],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      confirmPassword: [''],
    },
    { validators: matchValidator('password', 'confirmPassword') },
  );
  readonly submitting = signal(false);
  readonly generalErrors = signal<string[]>([]);

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting.set(true);
    this.generalErrors.set([]);

    this.auth.register(this.form.getRawValue()).subscribe({
      next: () => void this.router.navigateByUrl(this.auth.homeUrl()),
      error: (err: unknown) => {
        this.submitting.set(false);
        this.generalErrors.set(applyServerErrors(this.form, err));
      },
    });
  }
}
