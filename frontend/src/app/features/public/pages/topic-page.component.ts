import { Component, effect, inject, input, signal } from '@angular/core';
import { CourseGridComponent } from '../components/course-grid.component';
import { CatalogApi } from '../data-access/catalog.api';
import { CourseCard } from '../data-access/catalog.models';

/** Legacy Customer/Home/TopicCourse?word=: courses whose keyword or name contains the word. */
@Component({
  selector: 'app-topic-page',
  imports: [CourseGridComponent],
  styleUrl: './topic-page.component.scss',
  template: `
    <section id="courses" class="courses container p-3 mb-5">
      <div class="mb-4"><h5 class="fw-bolder">Courses That have <span class="text-warning">{{ word() }}</span></h5></div>
      <app-course-grid [courses]="courses()" emptyText="No Result Found ..." />
    </section>
  `,
})
export class TopicPageComponent {
  private readonly api = inject(CatalogApi);
  readonly word = input.required<string>();
  readonly courses = signal<CourseCard[]>([]);

  constructor() {
    effect(() => this.api.topic(this.word()).subscribe((c) => this.courses.set(c)));
  }
}
