import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from '../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../shared/forms.util';
import { matchValidator } from '../../../shared/validators';
import { AuthApi } from '../data-access/auth.api';

/**
 * "Teach in BFCAI" modal (legacy Home/_RegisterPartial).
 * Open it with: inject(NgbModal).open(InstructorRegisterModalComponent, { size: 'lg' })
 */
@Component({
  selector: 'app-instructor-register-modal',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  templateUrl: './instructor-register-modal.component.html',
  styleUrl: './instructor-register-modal.component.scss',
})
export class InstructorRegisterModalComponent {
  readonly modal = inject(NgbActiveModal);
  private readonly api = inject(AuthApi);
  private readonly notify = inject(NotifyService);

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
    this.api.registerInstructor(this.form.getRawValue()).subscribe({
      next: () => {
        this.notify.success('Your instructor account was created. You can log in now.');
        this.modal.close(true);
      },
      error: (err: unknown) => {
        this.submitting.set(false);
        this.generalErrors.set(applyServerErrors(this.form, err));
      },
    });
  }
}
