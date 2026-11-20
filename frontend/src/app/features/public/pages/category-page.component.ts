import { Component, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CourseGridComponent } from '../components/course-grid.component';
import { CatalogApi } from '../data-access/catalog.api';
import { Category } from '../data-access/catalog.models';

/** Legacy Customer/Home/Category/{id}: department courses + popular topics (keywords). */
@Component({
  selector: 'app-category-page',
  imports: [RouterLink, CourseGridComponent],
  styleUrl: './category-page.component.scss',
  template: `
    @if (category(); as c) {
      <section class="category-details px-xl-5">
        <div class="category-details-header"><h4>{{ c.name }} | {{ c.courses.length }} Courses</h4></div>
        <div class="w-75 px-3 category-details-description"><p>{{ c.description }}</p></div>
      </section>

      <section id="courses" class="courses container p-3 mb-5">
        <div class="mb-4"><h5>Top Courses for You in {{ c.name }}</h5></div>
        <app-course-grid [courses]="c.courses" />
      </section>

      <section class="topic-container container mb-5">
        <div class="topic-header mb-4"><h5>Popular Topic</h5></div>
        <div class="topics">
          @for (topic of c.keyWords; track topic) {
            <div class="one-topic">
              <a [routerLink]="['/topic', topic]" style="border-color: lightgray" class="btn text-black btn-outline-light">{{ topic }}</a>
            </div>
          }
        </div>
      </section>
    }
  `,
})
export class CategoryPageComponent {
  private readonly api = inject(CatalogApi);
  readonly id = input.required<string>();
  readonly category = signal<Category | null>(null);

  constructor() {
    effect(() => this.api.category(+this.id()).subscribe((c) => this.category.set(c)));
  }
}
