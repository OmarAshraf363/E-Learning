import { Component, inject, OnInit, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { applyServerErrors } from '../forms.util';
import { FormErrorsComponent } from './form-errors.component';

export interface FormModalField {
  name: string;
  label: string;
  type?: 'text' | 'textarea' | 'date' | 'datetime-local' | 'url' | 'number' | 'file';
  required?: boolean;
  /** File input "accept" attribute, e.g. "video/*". */
  accept?: string;
  placeholder?: string;
}

export interface FormModalOptions<TValue> {
  title: string;
  fields: FormModalField[];
  /** Initial values (edit mode). */
  value?: object;
  submitText?: string;
  /** Saves the values; a rejected promise shows the API validation errors in the form. */
  submit: (value: TValue) => Promise<unknown>;
}

/**
 * Generic modal form for small dialogs (legacy partial-view modals: curriculum section, reference,
 * quiz, video, assignment, feedback...). Open it with openFormModal(modal, options).
 * File fields produce a File (or null when nothing was chosen).
 */
@Component({
  selector: 'app-form-modal',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  template: `
    <form [formGroup]="form" (ngSubmit)="save()" novalidate>
      <div class="modal-header bg-dark text-light">
        <h5 class="modal-title">{{ options.title }}</h5>
        <button type="button" class="btn-close btn-close-white" aria-label="Close" (click)="modal.dismiss()"></button>
      </div>
      <div class="modal-body">
        @for (message of generalErrors(); track message) { <div class="text-danger mb-2">{{ message }}</div> }
        @for (field of options.fields; track field.name) {
          <div class="mb-3">
            <label class="form-label" [for]="'fm-' + field.name">{{ field.label }}</label>
            @switch (field.type) {
              @case ('textarea') {
                <textarea class="form-control" rows="4" [id]="'fm-' + field.name" [formControlName]="field.name" [placeholder]="field.placeholder ?? ''"></textarea>
              }
              @case ('file') {
                <input class="form-control" type="file" [id]="'fm-' + field.name" [accept]="field.accept ?? ''" (change)="onFile(field.name, $event)" />
              }
              @default {
                <input class="form-control" [type]="field.type ?? 'text'" [id]="'fm-' + field.name" [formControlName]="field.name" [placeholder]="field.placeholder ?? ''" />
              }
            }
            <app-form-errors [control]="form.controls[field.name]" />
          </div>
        }
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" (click)="modal.dismiss()">Cancel</button>
        <button type="submit" class="btn btn-primary" [disabled]="saving()">
          @if (saving()) { <span class="spinner-border spinner-border-sm me-2"></span> }
          {{ options.submitText ?? 'Save' }}
        </button>
      </div>
    </form>
  `,
})
export class FormModalComponent<TValue = Record<string, unknown>> implements OnInit {
  readonly modal = inject(NgbActiveModal);

  /** Set by openFormModal(). */
  options!: FormModalOptions<TValue>;

  form = new FormGroup<Record<string, FormControl<unknown>>>({});
  readonly saving = signal(false);
  readonly generalErrors = signal<string[]>([]);

  ngOnInit(): void {
    for (const field of this.options.fields) {
      const validators: ValidatorFn[] = field.required ? [Validators.required] : [];
      const initial = field.type === 'file' ? null : ((this.options.value as Record<string, unknown> | undefined)?.[field.name] ?? '');
      this.form.addControl(field.name, new FormControl<unknown>(initial, validators));
    }
  }

  onFile(name: string, event: Event): void {
    const control = this.form.controls[name];
    control.setValue((event.target as HTMLInputElement).files?.[0] ?? null);
    control.markAsTouched();
  }

  async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    try {
      const result = await this.options.submit(this.form.getRawValue() as TValue);
      this.modal.close(result);
    } catch (err) {
      this.generalErrors.set(applyServerErrors(this.form, err));
    } finally {
      this.saving.set(false);
    }
  }
}

/** Opens the generic form modal; resolves with the submit result, or undefined when dismissed. */
export function openFormModal<TValue>(modal: NgbModal, options: FormModalOptions<TValue>): Promise<unknown> {
  const ref = modal.open(FormModalComponent, { centered: true, size: 'lg' });
  (ref.componentInstance as FormModalComponent<TValue>).options = options;
  return ref.result.catch(() => undefined);
}
