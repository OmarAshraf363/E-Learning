import { Component, input } from '@angular/core';
import { CourseCard } from '../data-access/catalog.models';
import { CourseCardComponent } from './course-card.component';

/** A row of course cards (legacy ".top-courses"). */
@Component({
  selector: 'app-course-grid',
  imports: [CourseCardComponent],
  template: `
    <div class="top-courses">
      @for (course of courses(); track course.id) {
        <app-course-card [course]="course" />
      } @empty {
        @if (emptyText()) { <h6>{{ emptyText() }}</h6> }
      }
    </div>
  `,
})
export class CourseGridComponent {
  readonly courses = input.required<CourseCard[]>();
  readonly emptyText = input<string | null>(null);
}
