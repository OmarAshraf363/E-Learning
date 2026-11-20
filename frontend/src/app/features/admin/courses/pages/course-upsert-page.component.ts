import { Component, inject, input, OnInit, signal, viewChild } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { NotifyService } from '../../../../core/ui/notify.service';
import { CourseFormComponent } from '../../../../shared/course-form/course-form.component';
import { CourseFormValue, CourseRequest } from '../../../../shared/course-form/course-form.models';
import { LookupsApi } from '../../shared/lookups.api';
import { CoursesApi } from '../courses.api';
import { CoursesStore } from '../courses.store';

/** Legacy Admin/Course/UpSert. Routes: courses/new (?deptId= | ?trackId=) and courses/:id */
@Component({
  selector: 'app-course-upsert-page',
  imports: [CourseFormComponent],
  template: `
    <app-course-form [value]="course()" [departments]="departments()" [tracks]="tracks()" [instructors]="instructors()"
                     [presetDepartmentId]="deptId() ? +deptId()! : null" [presetTrackId]="trackId() ? +trackId()! : null"
                     [saving]="saving()" cancelLink="/admin/courses" (save)="save($event)" />
  `,
})
export class CourseUpsertPageComponent implements OnInit {
  private readonly api = inject(CoursesApi);
  private readonly store = inject(CoursesStore);
  private readonly lookups = inject(LookupsApi);
  private readonly router = inject(Router);
  private readonly notify = inject(NotifyService);
  private readonly formComponent = viewChild.required(CourseFormComponent);

  readonly id = input<string>();
  readonly deptId = input<string>();
  readonly trackId = input<string>();

  readonly instructors = toSignal(this.lookups.instructors(), { initialValue: [] });
  readonly departments = toSignal(this.lookups.departments(), { initialValue: [] });
  readonly tracks = toSignal(this.lookups.tracks(), { initialValue: [] });
  readonly course = signal<CourseFormValue | null>(null);
  readonly saving = signal(false);

  ngOnInit(): void {
    const id = this.id();
    if (id) this.api.getById(+id).subscribe((c) => this.course.set(c));
  }

  async save(request: CourseRequest): Promise<void> {
    this.saving.set(true);
    try {
      await this.store.save(request, this.id() ? +this.id()! : undefined);
      this.notify.success(this.id() ? 'Edited successfully' : 'Added successfully');
      void this.router.navigateByUrl('/admin/courses');
    } catch (err) {
      this.formComponent().showServerErrors(err);
    } finally {
      this.saving.set(false);
    }
  }
}
