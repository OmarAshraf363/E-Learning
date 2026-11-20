import { Component, inject, input, OnInit, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../shared/forms.util';
import { InstructorApi } from '../data-access/instructor.api';

/** Legacy Instructor/Exam/Upsert. Routes: exams/new and exams/:id */
@Component({
  selector: 'app-exam-form-page',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  template: `
    <div class="container mt-4" style="max-width: 700px">
      <h4>{{ id() ? 'Edit ' + title() + ' Exam' : 'Create New Exam' }}</h4>
      <form [formGroup]="form" (ngSubmit)="save()" novalidate>
        <div class="form-group mb-3">
          <label for="title" class="control-label">Title</label>
          <input id="title" formControlName="title" class="form-control" />
          <app-form-errors [control]="form.controls.title" />
        </div>
        <div class="form-group mb-3">
          <label for="examDate" class="control-label">ExamDate</label>
          <input id="examDate" type="datetime-local" formControlName="examDate" class="form-control" />
          <app-form-errors [control]="form.controls.examDate" />
        </div>
        <div class="form-group mb-3">
          <label for="courseId" class="control-label">Course</label>
          <select id="courseId" formControlName="courseId" class="form-control">
            <option [ngValue]="null">Select Course</option>
            @for (course of courses(); track course.id) { <option [ngValue]="course.id">{{ course.name }}</option> }
          </select>
          <app-form-errors [control]="form.controls.courseId" />
        </div>
        <button type="submit" class="btn btn-primary" [disabled]="saving()">{{ id() ? 'Save Changes' : 'Create' }}</button>
      </form>
    </div>
  `,
})
export class ExamFormPageComponent implements OnInit {
  private readonly api = inject(InstructorApi);
  private readonly router = inject(Router);
  private readonly notify = inject(NotifyService);

  readonly id = input<string>();
  readonly courses = toSignal(this.api.courseLookup(), { initialValue: [] });
  readonly title = signal('');
  readonly saving = signal(false);

  readonly form = inject(FormBuilder).nonNullable.group({
    title: ['', Validators.required],
    examDate: ['', Validators.required],
    courseId: new FormControl<number | null>(null, Validators.required),
  });

  ngOnInit(): void {
    const id = this.id();
    if (id) {
      this.api.exam(+id).subscribe((e) => {
        this.title.set(e.title);
        this.form.patchValue({ ...e, examDate: e.examDate.slice(0, 16) });
      });
    }
  }

  async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const v = this.form.getRawValue();
    const body = { title: v.title, examDate: v.examDate, courseId: v.courseId! };
    try {
      await firstValueFrom(this.id() ? this.api.updateExam(+this.id()!, body) : this.api.createExam(body));
      this.notify.success('Exam saved');
      void this.router.navigateByUrl('/instructor/exams');
    } catch (err) {
      applyServerErrors(this.form, err);
    } finally {
      this.saving.set(false);
    }
  }
}
