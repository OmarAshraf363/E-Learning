import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from '../../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../../shared/forms.util';
import { TracksApi } from '../tracks.api';
import { TracksStore } from '../tracks.store';

/** Create / edit track modal (legacy Track/UpSert). Set `id` on the instance for edit mode. */
@Component({
  selector: 'app-track-form-modal',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  template: `
    <form [formGroup]="form" (ngSubmit)="save()" novalidate>
      <div class="modal-header">
        <h5 class="modal-title">{{ id ? 'Edit ' + name() + ' Track' : 'Create New Track' }}</h5>
        <button type="button" class="btn-close" aria-label="Close" (click)="modal.dismiss()"></button>
      </div>
      <div class="modal-body">
        @for (message of generalErrors(); track message) { <div class="text-danger">{{ message }}</div> }
        <div class="form-group mb-2">
          <label for="t-name" class="control-label">Name</label>
          <input id="t-name" formControlName="name" class="form-control" />
          <app-form-errors [control]="form.controls.name" />
        </div>
        <div class="form-group mb-2">
          <label for="t-cover" class="control-label">Cover</label>
          <input id="t-cover" type="file" accept="image/*" class="form-control" (change)="onFile($event)" />
          <app-form-errors [control]="form.controls.cover" />
        </div>
        <div class="form-group mb-2">
          <label for="t-desc" class="control-label">Description</label>
          <input id="t-desc" formControlName="description" class="form-control" />
          <app-form-errors [control]="form.controls.description" />
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
export class TrackFormModalComponent implements OnInit {
  readonly modal = inject(NgbActiveModal);
  private readonly api = inject(TracksApi);
  private readonly store = inject(TracksStore);
  private readonly notify = inject(NotifyService);

  id?: number;
  readonly name = signal('');
  readonly saving = signal(false);
  readonly generalErrors = signal<string[]>([]);

  readonly form = inject(FormBuilder).group({
    name: ['', Validators.required],
    description: ['', Validators.required],
    cover: [null as File | null],
  });

  ngOnInit(): void {
    if (this.id) {
      this.api.getById(this.id).subscribe((t) => {
        this.name.set(t.name);
        this.form.patchValue({ name: t.name, description: t.description });
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
      await this.store.save({ name: v.name!, description: v.description!, cover: v.cover }, this.id);
      this.notify.success(this.id ? 'Edited successfully' : 'Added successfully');
      this.modal.close(true);
    } catch (err) {
      this.generalErrors.set(applyServerErrors(this.form, err));
    } finally {
      this.saving.set(false);
    }
  }
}
