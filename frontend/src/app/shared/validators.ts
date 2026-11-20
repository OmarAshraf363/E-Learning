import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

/**
 * Form-group validator: `confirmField` must equal `field` (password confirmation).
 * The error is set on the confirm control as { server: message } so <app-form-errors> shows it.
 */
export function matchValidator(
  field: string,
  confirmField: string,
  message = 'The password and confirmation password do not match.',
): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const confirm = group.get(confirmField);
    if (!confirm) return null;

    const mismatch = group.get(field)?.value !== confirm.value;
    const errors = { ...(confirm.errors ?? {}) };
    if (mismatch) errors['server'] = message;
    else if (errors['server'] === message) delete errors['server'];
    confirm.setErrors(Object.keys(errors).length ? errors : null);
    return null;
  };
}
