import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../shared/forms.util';
import { LookupsApi } from '../shared/lookups.api';
import { SchedulesApi } from './schedules.api';

/** Legacy Admin/Schedule/UpSert. Routes: schedules/new and schedules/:id */
@Component({
  selector: 'app-schedule-upsert-page',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  templateUrl: './schedule-upsert-page.component.html',
  styles: `
    .card { background-color: #1e1e1e; color: #f8f9fa; border: none; border-radius: 10px; }
    .card-header { background-color: #2d2d2d; }
    .form-label, .section-title { color: #adb5bd; }
    .section-title { font-weight: 600; margin-bottom: 1rem; }
    .form-control, .form-select { background-color: #2b2b2b; color: #f8f9fa; border-color: #444; }
  `,
})
export class ScheduleUpsertPageComponent implements OnInit {
  private readonly api = inject(SchedulesApi);
  private readonly lookups = inject(LookupsApi);
  private readonly router = inject(Router);
  private readonly notify = inject(NotifyService);

  readonly id = input<string>();
  readonly isEdit = computed(() => !!this.id());
  readonly saving = signal(false);

  readonly days = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];
  readonly courses = toSignal(this.lookups.courses(), { initialValue: [] });
  /** Business rule (second plan): only instructors can be picked (legacy listed every user). */
  readonly instructors = toSignal(this.lookups.instructors(), { initialValue: [] });
  readonly rooms = toSignal(this.lookups.rooms(), { initialValue: [] });
  readonly departments = toSignal(this.lookups.departments(), { initialValue: [] });
  readonly years = toSignal(this.lookups.academicYears(), { initialValue: [] });

  readonly form = inject(FormBuilder).nonNullable.group({
    courseId: new FormControl<number | null>(null, Validators.required),
    instructorId: new FormControl<string | null>(null),
    roomId: new FormControl<number | null>(null),
    dayOfWeek: ['', Validators.required],
    startTime: ['', Validators.required],
    endTime: ['', Validators.required],
    acadmicYearId: new FormControl<number | null>(null),
    departmentId: new FormControl<number | null>(null),
  });

  ngOnInit(): void {
    const id = this.id();
    if (id) this.api.getById(+id).subscribe((s) => this.form.patchValue(s));
  }

  async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const body = { ...this.form.getRawValue(), courseId: this.form.controls.courseId.value! };
    try {
      await firstValueFrom(this.id() ? this.api.update(+this.id()!, body) : this.api.create(body));
      this.notify.success('Schedule saved');
      void this.router.navigateByUrl('/admin/schedules');
    } catch (err) {
      applyServerErrors(this.form, err);
    } finally {
      this.saving.set(false);
    }
  }

  async remove(): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await firstValueFrom(this.api.delete(+this.id()!));
    void this.router.navigateByUrl('/admin/schedules');
  }
}
