import { Component, computed, input, output, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AdminCourseRow } from './course.models';

/**
 * Courses table with search + "New Course" (legacy Course/Index and Department/Courses).
 * Presentational: data in via inputs, delete out via (remove).
 */
@Component({
  selector: 'app-courses-table',
  imports: [RouterLink],
  styleUrl: '../shared/admin-table.scss',
  template: `
    <div class="container mt-4">
      <div class="head d-flex justify-content-between align-items-center mb-4">
        <h3 class="text-white">
          @if (title()) { <span class="text-warning">{{ title() }}</span> } Courses List
        </h3>
        <div class="search-container d-flex align-items-center w-50">
          <div class="input-group">
            <input type="search" class="form-control search" placeholder="Search..." (input)="search.set($any($event.target).value)" />
            <span class="input-group-text search-icon"><i class="fa-solid fa-magnifying-glass"></i></span>
          </div>
        </div>
        <a routerLink="/admin/courses/new" [queryParams]="newQuery()" class="btn new-course-btn"><i class="fas fa-plus"></i> New Course</a>
      </div>

      <table class="table table-hover">
        <thead>
          <tr><th>Course ID</th><th>Course Name</th><th>Department</th><th>Instructor</th><th>Actions</th></tr>
        </thead>
        <tbody>
          @for (course of rows(); track course.id) {
            <tr>
              <td>{{ course.id }}</td>
              <td>{{ course.name }}</td>
              <td>{{ course.departmentName }}</td>
              <td>{{ course.instructorName }}</td>
              <td class="d-flex gap-1">
                <a [routerLink]="['/admin/courses', course.id, 'details']" class="btn btn-info btn-sm">Details</a>
                <a [routerLink]="['/admin/courses', course.id]" class="btn btn-warning btn-sm">Edit</a>
                <button type="button" class="btn btn-danger btn-sm" (click)="remove.emit(course.id)">Delete</button>
              </td>
            </tr>
          } @empty {
            <tr><td colspan="5" class="text-center text-muted">No courses found.</td></tr>
          }
        </tbody>
      </table>
    </div>
  `,
})
export class CoursesTableComponent {
  readonly courses = input.required<AdminCourseRow[]>();
  readonly title = input<string | null>(null);
  /** Query params of the "New Course" link, e.g. { deptId: 1 }. */
  readonly newQuery = input<Record<string, number> | null>(null);
  readonly remove = output<number>();

  readonly search = signal('');
  readonly rows = computed(() => {
    const term = this.search().toLowerCase();
    return this.courses().filter((c) => c.name.toLowerCase().includes(term));
  });
}
