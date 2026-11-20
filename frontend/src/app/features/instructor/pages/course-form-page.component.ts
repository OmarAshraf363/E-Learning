import { Component, inject, input, OnInit, signal, viewChild } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { firstValueFrom, map } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { CourseFormComponent } from '../../../shared/course-form/course-form.component';
import { CourseFormValue, CourseRequest } from '../../../shared/course-form/course-form.models';
import { InstructorApi } from '../data-access/instructor.api';

/**
 * Legacy Instructor/InstructorCourse/UpSert (UpsertCourseByInstructor view).
 * Same form as the admin one, without the instructor select (always the current instructor).
 */
@Component({
  selector: 'app-instructor-course-form-page',
  imports: [CourseFormComponent],
  template: `
    <app-course-form [value]="course()" [departments]="lookups().departments" [tracks]="lookups().tracks"
                     [saving]="saving()" cancelLink="/instructor/courses" (save)="save($event)" />
  `,
})
export class CourseFormPageComponent implements OnInit {
  private readonly api = inject(InstructorApi);
  private readonly router = inject(Router);
  private readonly notify = inject(NotifyService);
  private readonly formComponent = viewChild.required(CourseFormComponent);

  readonly id = input<string>();
  readonly lookups = toSignal(this.api.formLookups().pipe(map((l) => l)), { initialValue: { departments: [], tracks: [] } });
  readonly course = signal<CourseFormValue | null>(null);
  readonly saving = signal(false);

  ngOnInit(): void {
    const id = this.id();
    if (id) this.api.course(+id).subscribe((c) => this.course.set(c));
  }

  async save(request: CourseRequest): Promise<void> {
    this.saving.set(true);
    try {
      const id = this.id();
      await firstValueFrom(id ? this.api.updateCourse(+id, request) : this.api.createCourse(request));
      this.notify.success(id ? 'Edited successfully' : 'Added successfully');
      void this.router.navigateByUrl('/instructor/courses');
    } catch (err) {
      this.formComponent().showServerErrors(err);
    } finally {
      this.saving.set(false);
    }
  }
}
