import { Component, input } from '@angular/core';
import { AbstractControl } from '@angular/forms';

/**
 * Shows validation messages under a form field: client-side validators and server errors.
 * Server errors (400 ProblemDetails) are put on controls with `applyServerErrors(form, error)`.
 *   <input formControlName="name" class="form-control" />
 *   <app-form-errors [control]="form.controls.name" />
 */
@Component({
  selector: 'app-form-errors',
  template: `
    @if (control().invalid && (control().touched || control().errors?.['server'])) {
      @for (message of messages(); track message) {
        <div class="text-danger small">{{ message }}</div>
      }
    }
  `,
})
export class FormErrorsComponent {
  readonly control = input.required<AbstractControl>();

  messages(): string[] {
    const errors = this.control().errors ?? {};
    return Object.entries(errors).map(([key, value]) => {
      switch (key) {
        case 'server':
          return String(value);
        case 'required':
          return 'This field is required.';
        case 'email':
          return 'Please enter a valid email.';
        case 'minlength':
          return `Minimum length is ${(value as { requiredLength: number }).requiredLength}.`;
        case 'maxlength':
          return `Maximum length is ${(value as { requiredLength: number }).requiredLength}.`;
        case 'min':
          return `Minimum value is ${(value as { min: number }).min}.`;
        default:
          return 'Invalid value.';
      }
    });
  }
}
