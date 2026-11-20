import { Component, effect, inject, input, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../../core/ui/notify.service';
import { CoursesTableComponent } from '../../courses/courses-table.component';
import { CoursesApi } from '../../courses/courses.api';
import { AdminCourseRow } from '../department.models';
import { DepartmentsApi } from '../departments.api';

/** Legacy Admin/Department/Courses. Route: departments/:id/courses */
@Component({
  selector: 'app-department-courses-page',
  imports: [CoursesTableComponent],
  template: `
    <app-courses-table [courses]="courses()" [title]="courses()[0]?.departmentName ?? null"
                       [newQuery]="{ deptId: +id() }" (remove)="remove($event)" />
  `,
})
export class DepartmentCoursesPageComponent {
  private readonly api = inject(DepartmentsApi);
  private readonly coursesApi = inject(CoursesApi);
  private readonly notify = inject(NotifyService);

  readonly id = input.required<string>();
  readonly courses = signal<AdminCourseRow[]>([]);

  constructor() {
    effect(() => this.load());
  }

  load(): void {
    this.api.courses(+this.id()).subscribe((c) => this.courses.set(c));
  }

  async remove(courseId: number): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await firstValueFrom(this.coursesApi.delete(courseId));
    this.load();
  }
}
