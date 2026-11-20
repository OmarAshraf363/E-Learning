import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from '../../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../../shared/forms.util';
import { DepartmentsApi } from '../departments.api';
import { DepartmentsStore } from '../departments.store';

/**
 * Create / edit department modal (legacy Department/UpSert partial).
 *   const ref = modal.open(DepartmentFormModalComponent); ref.componentInstance.id = 5;
 */
@Component({
  selector: 'app-department-form-modal',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  template: `
    <form [formGroup]="form" (ngSubmit)="save()" novalidate>
      <div class="modal-header">
        <h5 class="modal-title">{{ id ? 'Edit ' + name() + ' Category' : 'Create New Category' }}</h5>
        <button type="button" class="btn-close" aria-label="Close" (click)="modal.dismiss()"></button>
      </div>
      <div class="modal-body">
        @for (message of generalErrors(); track message) { <div class="text-danger">{{ message }}</div> }
        <div class="form-group mb-2">
          <label for="d-name" class="control-label">DepartmentName</label>
          <input id="d-name" formControlName="departmentName" class="form-control" />
          <app-form-errors [control]="form.controls.departmentName" />
        </div>
        <div class="form-group mb-2">
          <label for="d-desc" class="control-label">Description</label>
          <input id="d-desc" formControlName="departmentDescription" class="form-control" />
          <app-form-errors [control]="form.controls.departmentDescription" />
        </div>
        <div class="form-group mb-2">
          <label for="d-cover" class="control-label">Cover</label>
          <input id="d-cover" type="file" accept="image/*" class="form-control" (change)="onFile($event)" />
          <app-form-errors [control]="form.controls.cover" />
        </div>
      </div>
      <div class="modal-footer">
        <button type="submit" class="btn btn-primary" [disabled]="saving()">{{ id ? 'Save Changes' : 'Create' }}</button>
        <button type="button" class="btn btn-secondary" (click)="modal.dismiss()">Close</button>
      </div>
    </form>
  `,
  styles: `
    .form-group input { background-color: #303030; color: lightgray; }
  `,
})
export class DepartmentFormModalComponent implements OnInit {
  readonly modal = inject(NgbActiveModal);
  private readonly api = inject(DepartmentsApi);
  private readonly store = inject(DepartmentsStore);
  private readonly notify = inject(NotifyService);

  /** Set by the opener for edit mode. */
  id?: number;

  readonly name = signal('');
  readonly saving = signal(false);
  readonly generalErrors = signal<string[]>([]);

  readonly form = inject(FormBuilder).group({
    departmentName: ['', Validators.required],
    departmentDescription: ['', Validators.required],
    cover: [null as File | null],
  });

  ngOnInit(): void {
    if (this.id) {
      this.api.getById(this.id).subscribe((d) => {
        this.name.set(d.name);
        this.form.patchValue({ departmentName: d.name, departmentDescription: d.description });
      });
    }
  }

  onFile(event: Event): void {
    this.form.controls.cover.setValue((event.target as HTMLInputElement).files?.[0] ?? null);
  }

  async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const v = this.form.getRawValue();
    try {
      await this.store.save({ departmentName: v.departmentName!, departmentDescription: v.departmentDescription!, cover: v.cover }, this.id);
      this.notify.success(this.id ? 'Edited successfully' : 'Added successfully');
      this.modal.close(true);
    } catch (err) {
      this.generalErrors.set(applyServerErrors(this.form, err));
    } finally {
      this.saving.set(false);
    }
  }
}
