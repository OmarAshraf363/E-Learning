import { TestBed } from '@angular/core/testing';
import { FormControl, Validators } from '@angular/forms';
import { FormErrorsComponent } from './form-errors.component';

describe('FormErrorsComponent', () => {
  it('shows messages only after the control is touched, and server errors immediately', async () => {
    const control = new FormControl('', Validators.required);
    const fixture = TestBed.createComponent(FormErrorsComponent);
    fixture.componentRef.setInput('control', control);
    await fixture.whenStable();
    expect(fixture.nativeElement.textContent.trim()).toBe('');

    control.markAsTouched();
    await fixture.whenStable();
    expect(fixture.nativeElement.textContent).toContain('This field is required.');

    const other = new FormControl('x');
    fixture.componentRef.setInput('control', other);
    other.setErrors({ server: 'Email already taken.' });
    await fixture.whenStable();
    expect(fixture.nativeElement.textContent).toContain('Email already taken.');
  });
});
