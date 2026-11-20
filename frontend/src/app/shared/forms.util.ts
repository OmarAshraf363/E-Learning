import { FormGroup } from '@angular/forms';
import { fieldErrors } from '../core/errors/error.interceptor';

/**
 * Copies API validation errors (400 ProblemDetails) onto matching form controls,
 * so <app-form-errors> shows them under each field. API field names are PascalCase
 * ("CourseName") while form controls are camelCase ("courseName").
 * Returns messages that did not match any control (show them at the top of the form).
 */
export function applyServerErrors(form: FormGroup, error: unknown): string[] {
  const unmatched: string[] = [];
  for (const [field, messages] of Object.entries(fieldErrors(error))) {
    const name = field.charAt(0).toLowerCase() + field.slice(1);
    const control = form.get(name);
    if (control) {
      control.setErrors({ server: messages.join(' ') });
      control.markAsTouched();
    } else unmatched.push(...messages);
  }
  return unmatched;
}
