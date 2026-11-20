import { Component, effect, inject, input, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { PagedResult } from '../../../core/models/paged-result';
import { NotifyService } from '../../../core/ui/notify.service';
import { openFormModal } from '../../../shared/components/form-modal.component';
import { PaginationComponent } from '../../../shared/components/pagination.component';
import { InstructorApi } from '../data-access/instructor.api';
import { InstructorCourseRow } from '../data-access/instructor.models';

/**
 * Legacy Instructor/InstructorCourse/Index (paged). "View" opens the curriculum manager,
 * "Add-Process" adds a curriculum section (legacy UpsertCourseCurriculum modal).
 */
@Component({
  selector: 'app-instructor-courses-page',
  imports: [RouterLink, PaginationComponent],
  template: `
    <div class="container mt-5">
      <div class="courses-header d-flex align-items-center justify-content-between">
        <h1 class="mb-4">Courses</h1>
        <a routerLink="/instructor/courses/new" class="btn btn-dark">New Course</a>
      </div>
      @if (result(); as r) {
        @if (r.items.length) {
          <table class="table table-striped">
            <thead><tr><th>Course Name</th><th>Students Enrolled</th><th>Process</th><th>Actions</th></tr></thead>
            <tbody>
              @for (course of r.items; track course.id) {
                <tr>
                  <td>{{ course.name }}</td>
                  <td class="text-center">{{ course.studentsEnrolled }}</td>
                  <td><a [routerLink]="['/instructor/courses', course.id, 'curriculum']" class="btn btn-outline-info">View</a></td>
                  <td class="d-flex gap-1">
                    <button type="button" class="btn btn-dark" (click)="addSection(course.id)">Add-Process</button>
                    <a [routerLink]="['/instructor/courses', course.id]" class="btn btn-outline-secondary"><i class="fas fa-edit"></i></a>
                  </td>
                </tr>
              }
            </tbody>
          </table>
          <app-pagination [page]="r.page" [totalPages]="r.totalPages" (pageChange)="goTo($event)" />
        } @else {
          <div class="alert alert-info" role="alert">You have no courses.</div>
        }
      }
    </div>
  `,
})
export class CoursesPageComponent {
  private readonly api = inject(InstructorApi);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);
  private readonly router = inject(Router);

  readonly page = input<string>();
  readonly result = signal<PagedResult<InstructorCourseRow> | null>(null);

  constructor() {
    effect(() => this.api.courses(this.page() ? +this.page()! : 1).subscribe((r) => this.result.set(r)));
  }

  goTo(page: number): void {
    void this.router.navigate([], { queryParams: { page } });
  }

  async addSection(courseId: number): Promise<void> {
    const saved = await openFormModal<{ title: string; content: string }>(this.modal, {
      title: 'Add New Curriculum',
      fields: [
        { name: 'title', label: 'Title', required: true, placeholder: 'Enter curriculum title' },
        { name: 'content', label: 'Content', type: 'textarea', required: true, placeholder: 'Enter curriculum content' },
      ],
      submit: (v) => firstValueFrom(this.api.addSection(courseId, v)),
    });
    if (saved) this.notify.success('Curriculum added');
  }
}
