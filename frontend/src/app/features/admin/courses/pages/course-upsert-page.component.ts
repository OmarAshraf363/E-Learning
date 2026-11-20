import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormArray, FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NotifyService } from '../../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../../shared/forms.util';
import { FileUrlPipe } from '../../../../shared/pipes/file-url.pipe';
import { LookupsApi } from '../../shared/lookups.api';
import { CoursesApi } from '../courses.api';
import { CoursesStore } from '../courses.store';

/**
 * Legacy Admin/Course/UpSert. Routes: courses/new (?deptId= | ?trackId=) and courses/:id
 * Key words / objectives / topics are dynamic lists (FormArray).
 */
@Component({
  selector: 'app-course-upsert-page',
  imports: [ReactiveFormsModule, RouterLink, FormErrorsComponent, FileUrlPipe],
  templateUrl: './course-upsert-page.component.html',
  styleUrl: './course-upsert-page.component.scss',
})
export class CourseUpsertPageComponent implements OnInit {
  private readonly api = inject(CoursesApi);
  private readonly store = inject(CoursesStore);
  private readonly lookups = inject(LookupsApi);
  private readonly router = inject(Router);
  private readonly notify = inject(NotifyService);
  private readonly fb = inject(FormBuilder).nonNullable;

  readonly id = input<string>();
  readonly deptId = input<string>();
  readonly trackId = input<string>();

  readonly instructors = toSignal(this.lookups.instructors(), { initialValue: [] });
  readonly departments = toSignal(this.lookups.departments(), { initialValue: [] });
  readonly tracks = toSignal(this.lookups.tracks(), { initialValue: [] });

  readonly isEdit = computed(() => !!this.id());
  readonly courseName = signal('');
  readonly currentCover = signal<string | null>(null);
  readonly saving = signal(false);
  readonly generalErrors = signal<string[]>([]);

  readonly presetDepartmentName = computed(() => this.departments().find((d) => d.id === +(this.deptId() ?? 0))?.name);
  readonly presetTrackName = computed(() => this.tracks().find((t) => t.id === +(this.trackId() ?? 0))?.name);

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
    instructorId: ['', Validators.required],
    trackId: new FormControl<number | null>(null),
    departmentId: new FormControl<number | null>(null, Validators.required),
    keyWords: this.fb.array<FormControl<string>>([]),
    learningObjectives: this.fb.array<FormControl<string>>([]),
    topicsCovered: this.fb.array<FormControl<string>>([]),
  });

  ngOnInit(): void {
    const id = this.id();
    if (id) {
      this.api.getById(+id).subscribe((c) => {
        this.courseName.set(c.courseName);
        this.currentCover.set(c.imgCover);
        const { imgCover: _cover, keyWords: _k, learningObjectives: _o, topicsCovered: _t, ...values } = c;
        this.form.patchValue({
          ...values,
          startDate: c.startDate?.slice(0, 10) ?? '',
          endDate: c.endDate?.slice(0, 10) ?? '',
          demoVideoUrl: c.demoVideoUrl ?? '',
        });
        c.keyWords.forEach((v) => this.add(this.form.controls.keyWords, v));
        c.learningObjectives.forEach((v) => this.add(this.form.controls.learningObjectives, v));
        c.topicsCovered.forEach((v) => this.add(this.form.controls.topicsCovered, v));
      });
    } else {
      if (this.deptId()) this.form.patchValue({ departmentId: +this.deptId()! });
      if (this.trackId()) this.form.patchValue({ trackId: +this.trackId()! });
    }
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

  async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    this.generalErrors.set([]);
    const v = this.form.getRawValue();
    try {
      await this.store.save(
        { ...v, startDate: v.startDate || null, endDate: v.endDate || null, demoVideoUrl: v.demoVideoUrl || null },
        this.id() ? +this.id()! : undefined,
      );
      this.notify.success(this.isEdit() ? 'Edited successfully' : 'Added successfully');
      void this.router.navigateByUrl('/admin/courses');
    } catch (err) {
      this.generalErrors.set(applyServerErrors(this.form, err));
    } finally {
      this.saving.set(false);
    }
  }
}
