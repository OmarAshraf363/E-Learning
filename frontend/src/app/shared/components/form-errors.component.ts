import { Component, computed, input } from '@angular/core';
import { toObservable, toSignal } from '@angular/core/rxjs-interop';
import { AbstractControl, ValidationErrors } from '@angular/forms';
import { map, startWith, switchMap } from 'rxjs';

interface ControlState {
  show: boolean;
  errors: ValidationErrors | null;
}

/**
 * Shows validation messages under a form field: client-side validators and server errors.
 * Server errors (400 ProblemDetails) are put on controls with `applyServerErrors(form, error)`.
 *   <input formControlName="name" class="form-control" />
 *   <app-form-errors [control]="form.controls.name" />
 *
 * It listens to the control's events (value / status / touched), so it updates correctly
 * with signal-based (OnPush / zoneless) change detection.
 */
@Component({
  selector: 'app-form-errors',
  template: `
    @if (state().show) {
      @for (message of messages(); track message) {
        <div class="text-danger small">{{ message }}</div>
      }
    }
  `,
})
export class FormErrorsComponent {
  readonly control = input.required<AbstractControl>();

  readonly state = toSignal(
    toObservable(this.control).pipe(
      switchMap((control) =>
        control.events.pipe(
          startWith(null),
          map(
            (): ControlState => ({
              show: control.invalid && (control.touched || !!control.errors?.['server']),
              errors: control.errors,
            }),
          ),
        ),
      ),
    ),
    { initialValue: { show: false, errors: null } as ControlState },
  );

  readonly messages = computed(() =>
    Object.entries(this.state().errors ?? {}).map(([key, value]) => {
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
    }),
  );
}
