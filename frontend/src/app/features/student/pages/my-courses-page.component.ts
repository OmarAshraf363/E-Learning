import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { StarRatingComponent } from '../../../shared/components/star-rating.component';
import { StudentApi } from '../data-access/student.api';

/** Legacy Student/Courses/Index ("My Courses" + progress). A card opens the course player. */
@Component({
  selector: 'app-my-courses-page',
  imports: [RouterLink, FileUrlPipe, StarRatingComponent],
  styleUrl: './my-courses-page.component.scss',
  template: `
    <section class="courses container p-3 mb-5">
      <div class="mt-3 d-flex justify-content-between align-items-center">
        <h3 class="fw-bolder">My Courses</h3>
        <span>{{ courses().length }} Courses</span>
      </div>
      <div class="top-courses">
        @for (course of courses(); track course.id) {
          <a class="one-course text-decoration-none text-reset" [routerLink]="['/student/courses', course.id]">
            <div class="course-video mb-3"><img [src]="course.cover | fileUrl: 'images/online-courses.png'" [alt]="course.name" /></div>
            <div class="course-title"><h5>{{ course.name }}</h5></div>
            <div class="w-75 instructor"><h6>{{ course.instructorName }}</h6></div>
            <div class="rating"><app-star-rating [rate]="course.rate" /></div>
            <div class="progress mt-3">
              <div class="progress-bar bg-success" role="progressbar" [style.width.%]="course.progressPercentage"
                   [attr.aria-valuenow]="course.progressPercentage" aria-valuemin="0" aria-valuemax="100">{{ course.progressPercentage }}%</div>
            </div>
          </a>
        } @empty {
          <p class="text-muted mt-3">You are not enrolled in any course yet.</p>
        }
      </div>
    </section>
  `,
})
export class MyCoursesPageComponent {
  readonly courses = toSignal(inject(StudentApi).myCourses(), { initialValue: [] });
}
