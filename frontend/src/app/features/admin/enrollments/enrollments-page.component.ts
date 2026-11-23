import { Component, inject, signal, TemplateRef, viewChild } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbModal, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../shared/components/form-errors.component';
import { LookupsApi } from '../shared/lookups.api';
import { EnrollmentOverview, EnrollmentsApi } from './enrollments.api';
import { HttpErrorResponse } from '@angular/common/http';
import { ProblemDetails } from '../../../core/models/problem-details';

/**
 * Legacy Admin/Enrollment/Index: enroll by email, enroll via CSV, latest 10 enrollments.
 * (The legacy "Enroll Manually" dialog had no action and is not migrated.)
 */
@Component({
  selector: 'app-enrollments-page',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  templateUrl: './enrollments-page.component.html',
})
export class EnrollmentsPageComponent {
  private readonly api = inject(EnrollmentsApi);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);
  private readonly csvModal = viewChild.required<TemplateRef<unknown>>('csvModal');

  readonly courses = toSignal(inject(LookupsApi).courses(), { initialValue: [] });
  readonly overview = signal<EnrollmentOverview | null>(null);
  readonly success = signal<string | null>(null);
  readonly error = signal<string | null>(null);

  readonly form = inject(FormBuilder).nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    courseId: new FormControl<number | null>(null, Validators.required),
  });
  private csvFile: File | null = null;
  private modalRef?: NgbModalRef;

  constructor() {
    this.load();
  }

  load(): void {
    this.api.overview().subscribe((o) => this.overview.set(o));
  }

  async enroll(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.success.set(null);
    this.error.set(null);
    const { email, courseId } = this.form.getRawValue();
    try {
      await firstValueFrom(this.api.enrollByEmail({ email, courseId: courseId! }));
      this.success.set('Student has been successfully enrolled.');
      this.form.reset();
      this.load();
    } catch (err) {
      if (err instanceof HttpErrorResponse) this.error.set((err.error as ProblemDetails)?.detail ?? 'Invalid data provided.');
    }
  }

  openCsv(): void {
    this.csvFile = null;
    this.modalRef = this.modal.open(this.csvModal());
  }

  onCsvSelected(event: Event): void {
    this.csvFile = (event.target as HTMLInputElement).files?.[0] ?? null;
  }

  async uploadCsv(): Promise<void> {
    if (!this.csvFile) return;
    const result = await firstValueFrom(this.api.uploadCsv(this.csvFile));
    this.modalRef?.close();
    this.notify.success(
      `${result.added} enrollment(s) added, ${result.alreadyEnrolled} already enrolled (not added again), ${result.skipped} line(s) skipped.`,
    );
    this.load();
  }
}
