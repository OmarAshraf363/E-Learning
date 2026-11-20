import { Component, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AdminCourseDetails } from '../course.models';
import { CoursesApi } from '../courses.api';

/** Legacy Admin/Course/Details. Route: courses/:id/details */
@Component({
  selector: 'app-course-details-page',
  imports: [RouterLink],
  template: `
    @if (course(); as c) {
      <div class="container mt-4">
        <h2 class="text-white mb-4">Course Details</h2>
        <div class="card bg-dark text-white">
          <div class="card-header"><h3 class="card-title">{{ c.name }}</h3></div>
          <div class="card-body">
            <dl class="row">
              <dt class="col-sm-3">Course ID:</dt><dd class="col-sm-9">{{ c.id }}</dd>
              <dt class="col-sm-3">Description:</dt><dd class="col-sm-9">{{ c.description }}</dd>
              <dt class="col-sm-3">Credits:</dt><dd class="col-sm-9">{{ c.credits }}</dd>
              <dt class="col-sm-3">Instructor:</dt><dd class="col-sm-9">{{ c.instructorName }}</dd>
              <dt class="col-sm-3">Department:</dt><dd class="col-sm-9">{{ c.departmentName }}</dd>
              <dt class="col-sm-3">Enrollments:</dt><dd class="col-sm-9">{{ c.enrollmentsCount }}</dd>
            </dl>
          </div>
          <div class="card-footer d-flex gap-2">
            <a [routerLink]="['/admin/courses', c.id]" class="btn btn-warning"><i class="fa-solid fa-pencil-alt"></i> Edit</a>
            <a routerLink="/admin/courses" class="btn btn-secondary"><i class="fa-solid fa-arrow-left"></i> Back to List</a>
          </div>
        </div>
      </div>
    }
  `,
  styles: `
    .card { border-radius: 8px; border: none; }
    .card-header { background-color: #1e1e1e; border-bottom: 1px solid #333; }
    .card-body { background-color: #121212; }
    .card-footer { background-color: #1e1e1e; border-top: 1px solid #333; }
    .btn { padding-inline: 15px; padding-block: 8px; border: none; border-radius: 5px; }
  `,
})
export class CourseDetailsPageComponent {
  private readonly api = inject(CoursesApi);
  readonly id = input.required<string>();
  readonly course = signal<AdminCourseDetails | null>(null);

  constructor() {
    effect(() => this.api.details(+this.id()).subscribe((c) => this.course.set(c)));
  }
}
