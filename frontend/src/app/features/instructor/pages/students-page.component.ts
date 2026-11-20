import { Component, effect, inject, input, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { openFormModal } from '../../../shared/components/form-modal.component';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { InstructorApi } from '../data-access/instructor.api';
import { CourseStudent } from '../data-access/instructor.models';

/** Legacy Instructor/Student/Index (+ InsertFeedBack modal). ?courseId= selects the course. */
@Component({
  selector: 'app-students-page',
  imports: [FileUrlPipe],
  template: `
    <div class="container mt-4">
      <h2 class="mb-4"><i class="fas fa-users-cog"></i> Manage Students</h2>
      <div class="mb-4" style="max-width: 400px">
        <label for="course" class="form-label">Select Course</label>
        <select id="course" class="form-select" (change)="selectCourse($any($event.target).value)">
          <option value="">-- Select Course --</option>
          @for (course of courses(); track course.id) {
            <option [value]="course.id" [selected]="'' + course.id === courseId()">{{ course.name }}</option>
          }
        </select>
      </div>

      <div class="row g-4">
        @for (student of students(); track student.id) {
          <div class="col-md-4 col-lg-3">
            <div class="card student-card text-center p-3 h-100 shadow-sm">
              <img [src]="student.picture | fileUrl: 'images/default-avatar.svg'" alt="Avatar" class="rounded-circle mx-auto mb-3" style="width: 90px; height: 90px; object-fit: cover" />
              <h5 class="fw-bold">{{ student.fullName }}</h5>
              <p class="mb-1"><i class="fas fa-envelope text-info"></i> {{ student.email }}</p>
              <p class="mb-2"><i class="fas fa-graduation-cap text-warning"></i> GPA: <strong>{{ student.gpa }}</strong></p>
              <button type="button" class="btn btn-dark btn-sm mt-2" (click)="feedback(student)"><i class="fas fa-comment-alt"></i> Add Feedback</button>
            </div>
          </div>
        } @empty {
          @if (courseId()) { <div class="alert alert-info">No students enrolled in this course.</div> }
        }
      </div>
    </div>
  `,
})
export class StudentsPageComponent {
  private readonly api = inject(InstructorApi);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);
  private readonly router = inject(Router);

  readonly courseId = input<string>();
  readonly courses = toSignal(this.api.courseLookup(), { initialValue: [] });
  readonly students = signal<CourseStudent[]>([]);

  constructor() {
    effect(() => {
      const id = this.courseId();
      if (id) this.api.students(+id).subscribe((s) => this.students.set(s));
      else this.students.set([]);
    });
  }

  selectCourse(value: string): void {
    void this.router.navigate([], { queryParams: { courseId: value || null } });
  }

  async feedback(student: CourseStudent): Promise<void> {
    const result = await openFormModal<{ content: string }>(this.modal, {
      title: 'Submit Feedback',
      fields: [{ name: 'content', label: 'Content', type: 'textarea', required: true }],
      submitText: 'Submit Feedback',
      submit: (v) => firstValueFrom(this.api.addFeedback({ studentId: student.id, content: v.content })),
    });
    if (result !== undefined) this.notify.success('Added Successfully');
  }
}
