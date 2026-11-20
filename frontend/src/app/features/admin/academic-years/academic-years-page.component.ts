import { Component, inject, Injectable, signal, TemplateRef, viewChild } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { CrudStore } from '../../../core/state/crud-store';
import { NotifyService } from '../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../shared/forms.util';
import { AcademicYear, AcademicYearRequest, AcademicYearsApi } from './academic-years.api';

@Injectable({ providedIn: 'root' })
export class AcademicYearsStore extends CrudStore<AcademicYear, AcademicYearRequest> {
  constructor() {
    super(inject(AcademicYearsApi), (y) => y.id);
  }
}

/** Legacy Admin/AcadmicYear/Index + Upsert modal. */
@Component({
  selector: 'app-academic-years-page',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  template: `
    <div class="container mt-4">
      <h2 class="text-center text-white">Academic Years</h2>
      <button type="button" class="btn btn-primary mb-3" (click)="open()">Add New Academic Year</button>
      <table class="table table-bordered">
        <thead><tr><th>ID</th><th>Year</th><th>Actions</th></tr></thead>
        <tbody>
          @for (year of store.items(); track year.id) {
            <tr>
              <td>{{ year.id }}</td>
              <td>{{ year.yearName }}</td>
              <td class="d-flex gap-1">
                <button type="button" class="btn btn-warning" (click)="open(year)">Edit</button>
                <button type="button" class="btn btn-danger" (click)="remove(year.id)">Delete</button>
              </td>
            </tr>
          }
        </tbody>
      </table>
    </div>

    <ng-template #formModal>
      <form [formGroup]="form" (ngSubmit)="save()" novalidate>
        <div class="modal-header"><h2 class="text-center">{{ editingId() ? 'Edit Academic Year' : 'Create New Academic Year' }}</h2></div>
        <div class="modal-body">
          <div class="form-group">
            <label for="yearName">YearName</label>
            <input id="yearName" formControlName="yearName" class="form-control bg-dark text-light" placeholder="Like This 2020-2021" />
            <app-form-errors [control]="form.controls.yearName" />
          </div>
        </div>
        <div class="modal-footer">
          <button type="submit" class="btn btn-primary">{{ editingId() ? 'Update' : 'Create' }}</button>
          <button type="button" class="btn btn-secondary" (click)="modalRef?.dismiss()">Cancel</button>
        </div>
      </form>
    </ng-template>
  `,
})
export class AcademicYearsPageComponent {
  readonly store = inject(AcademicYearsStore);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);
  private readonly formModal = viewChild.required<TemplateRef<unknown>>('formModal');

  readonly editingId = signal<number | undefined>(undefined);
  readonly form = inject(FormBuilder).nonNullable.group({ yearName: ['', Validators.required] });
  modalRef?: NgbModalRef;

  constructor() {
    void this.store.load();
  }

  open(year?: AcademicYear): void {
    this.editingId.set(year?.id);
    this.form.reset({ yearName: year?.yearName ?? '' });
    this.modalRef = this.modal.open(this.formModal(), { centered: true });
  }

  async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    try {
      await this.store.save(this.form.getRawValue(), this.editingId());
      this.modalRef?.close();
      this.notify.success('Saved successfully');
    } catch (err) {
      applyServerErrors(this.form, err);
    }
  }

  async remove(id: number): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await this.store.remove(id);
  }
}
