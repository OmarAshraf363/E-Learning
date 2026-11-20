import { Component, computed, effect, inject, input, output, signal } from '@angular/core';
import { FormArray, FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { FormErrorsComponent } from '../components/form-errors.component';
import { applyServerErrors } from '../forms.util';
import { FileUrlPipe } from '../pipes/file-url.pipe';
import { CourseFormValue, CourseRequest, Option } from './course-form.models';

/**
 * The course form (legacy Admin/Course/UpSert and Instructor/UpsertCourseByInstructor).
 * Presentational: the page passes the data and dropdowns in and saves on (save).
 *  - instructors = null hides the instructor select (instructor area: always the current user).
 *  - Key words / objectives / topics are dynamic lists (FormArray).
 */
@Component({
  selector: 'app-course-form',
  imports: [ReactiveFormsModule, RouterLink, FormErrorsComponent, FileUrlPipe],
  templateUrl: './course-form.component.html',
  styleUrl: './course-form.component.scss',
})
export class CourseFormComponent {
  private readonly fb = inject(FormBuilder).nonNullable;

  /** Existing course (edit) or null (create). */
  readonly value = input<CourseFormValue | null>(null);
  readonly departments = input<Option[]>([]);
  readonly tracks = input<Option[]>([]);
  readonly instructors = input<Option<string>[] | null>(null);
  readonly presetDepartmentId = input<number | null>(null);
  readonly presetTrackId = input<number | null>(null);
  readonly saving = input(false);
  readonly cancelLink = input<string>('/');
  readonly save = output<CourseRequest>();

  readonly isEdit = computed(() => !!this.value());
  readonly courseName = computed(() => this.value()?.courseName ?? '');
  readonly currentCover = computed(() => this.value()?.imgCover ?? null);
  readonly presetDepartmentName = computed(() => this.departments().find((d) => d.id === this.presetDepartmentId())?.name);
  readonly presetTrackName = computed(() => this.tracks().find((t) => t.id === this.presetTrackId())?.name);
  readonly generalErrors = signal<string[]>([]);

  readonly form = this.fb.group({
    courseName: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.required, Validators.maxLength(1000)]],
    imgCover: new FormControl<File | null>(null),
    price: new FormControl<number | null>(null, Validators.min(0)),
    credits: new FormControl<number | null>(null, [Validators.required, Validators.min(1), Validators.max(50)]),
    rate: new FormControl<number | null>(null),
    demoVideoUrl: ['', Validators.required],
    numOfEnrollments: new FormControl<number | null>(null),
    startDate: ['', Validators.required],
    endDate: ['', Validators.required],
    instructorId: [''],
    trackId: new FormControl<number | null>(null),
    departmentId: new FormControl<number | null>(null, Validators.required),
    keyWords: this.fb.array<FormControl<string>>([]),
    learningObjectives: this.fb.array<FormControl<string>>([]),
    topicsCovered: this.fb.array<FormControl<string>>([]),
  });

  constructor() {
    // Instructor select is required only when it is shown (admin).
    effect(() => {
      const control = this.form.controls.instructorId;
      control.setValidators(this.instructors() ? Validators.required : null);
      control.updateValueAndValidity();
    });

    effect(() => {
      const course = this.value();
      if (course) this.fill(course);
    });

    effect(() => {
      if (this.isEdit()) return;
      if (this.presetDepartmentId()) this.form.patchValue({ departmentId: this.presetDepartmentId() });
      if (this.presetTrackId()) this.form.patchValue({ trackId: this.presetTrackId() });
    });
  }

  add(list: FormArray<FormControl<string>>, value = ''): void {
    list.push(this.fb.control(value));
  }

  removeAt(list: FormArray<FormControl<string>>, index: number): void {
    list.removeAt(index);
  }

  onFile(event: Event): void {
    this.form.controls.imgCover.setValue((event.target as HTMLInputElement).files?.[0] ?? null);
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.generalErrors.set([]);
    const v = this.form.getRawValue();
    this.save.emit({
      ...v,
      instructorId: v.instructorId || null,
      startDate: v.startDate || null,
      endDate: v.endDate || null,
      demoVideoUrl: v.demoVideoUrl || null,
    });
  }

  /** Called by the page when the API rejected the save (400 ProblemDetails). */
  showServerErrors(error: unknown): void {
    this.generalErrors.set(applyServerErrors(this.form, error));
  }

  private fill(c: CourseFormValue): void {
    const { imgCover: _cover, keyWords, learningObjectives, topicsCovered, ...values } = c;
    this.form.patchValue({
      ...values,
      startDate: c.startDate?.slice(0, 10) ?? '',
      endDate: c.endDate?.slice(0, 10) ?? '',
      demoVideoUrl: c.demoVideoUrl ?? '',
    });
    for (const [list, items] of [
      [this.form.controls.keyWords, keyWords],
      [this.form.controls.learningObjectives, learningObjectives],
      [this.form.controls.topicsCovered, topicsCovered],
    ] as const) {
      list.clear();
      items.forEach((item) => this.add(list, item));
    }
  }
}
