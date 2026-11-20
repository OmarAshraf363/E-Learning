import { Component, inject, input, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthService } from '../../../../core/auth/auth.service';
import { FormErrorsComponent } from '../../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../../shared/forms.util';
import { ProblemDetails } from '../../../../core/models/problem-details';

/** Legacy Identity/Account/Login page. */
@Component({
  selector: 'app-login-page',
  imports: [ReactiveFormsModule, RouterLink, FormErrorsComponent],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.scss',
})
export class LoginPageComponent {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  /** ?returnUrl=... (bound from the query string) */
  readonly returnUrl = input<string>();

  readonly form = inject(FormBuilder).nonNullable.group({
    email: ['', Validators.required],
    password: ['', Validators.required],
  });
  readonly submitting = signal(false);
  readonly error = signal<string | null>(null);

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting.set(true);
    this.error.set(null);

    const { email, password } = this.form.getRawValue();
    this.auth.login(email, password).subscribe({
      next: () => void this.router.navigateByUrl(this.returnUrl() || this.auth.homeUrl()),
      error: (err: unknown) => {
        this.submitting.set(false);
        if (err instanceof HttpErrorResponse && err.status === 401)
          this.error.set((err.error as ProblemDetails)?.detail ?? 'Invalid login attempt.');
        else applyServerErrors(this.form, err);
      },
    });
  }
}
