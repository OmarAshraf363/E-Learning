import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { CourseGridComponent } from '../components/course-grid.component';
import { CatalogApi } from '../data-access/catalog.api';
import { PublicTrack } from '../data-access/catalog.models';

/** Legacy Customer/Home/TrackDetails: track overview, student progress + missing courses, all courses. */
@Component({
  selector: 'app-track-page',
  imports: [FileUrlPipe, CourseGridComponent],
  styleUrl: './track-page.component.scss',
  template: `
    @if (track(); as t) {
      <div class="container my-5">
        <div class="track-overview mb-5">
          <div class="row align-items-center">
            <div class="col-lg-6 text-center text-lg-start">
              <h1 class="display-4 fw-bold text-primary">{{ t.title }}</h1>
              <p class="lead text-muted"><strong>{{ t.description }}</strong></p>
            </div>
            <div class="col-lg-6"><img [src]="t.cover | fileUrl" [alt]="t.title" class="img-fluid rounded shadow-sm" /></div>
          </div>
        </div>

        @if (t.coursesStudentDoesNotHave.length) {
          <section class="courses container p-3 mb-5">
            <div class="mb-1"><h4>Enroll in this courses now to achive <span class="text-primary">{{ t.title }}</span> track now</h4></div>
            <div class="progress progress-custom mb-3">
              <div class="progress-bar progress-bar-striped progress-bar-animated bg-warning" role="progressbar"
                   [style.width.%]="progress()" [attr.aria-valuenow]="progress()" aria-valuemin="0" aria-valuemax="100">{{ progress() }}%</div>
            </div>
            <app-course-grid [courses]="t.coursesStudentDoesNotHave" />
          </section>
        }

        <section class="courses container p-3 mb-5">
          <div class="mb-4"><h4>Track Courses</h4></div>
          <app-course-grid [courses]="t.courses" />
        </section>
      </div>
    }
  `,
})
export class TrackPageComponent {
  private readonly api = inject(CatalogApi);
  readonly id = input.required<string>();
  readonly track = signal<PublicTrack | null>(null);

  readonly progress = computed(() => {
    const t = this.track();
    return !t || t.numOfCourses === 0 ? 0 : Math.round((t.numOfCoursesStudentHas * 100) / t.numOfCourses);
  });

  constructor() {
    effect(() => this.api.track(+this.id()).subscribe((t) => this.track.set(t)));
  }
}
