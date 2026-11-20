import { CurrencyPipe } from '@angular/common';
import { Component, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { FileUrlPipe } from '../../../../shared/pipes/file-url.pipe';
import { TrackDetails } from '../track.models';
import { TracksApi } from '../tracks.api';

/** Legacy Track/Details (admin branch of Views/Shared/Details.cshtml). Route: tracks/:id */
@Component({
  selector: 'app-track-details-page',
  imports: [RouterLink, FileUrlPipe, CurrencyPipe],
  template: `
    @if (track(); as t) {
      <div class="container mt-4">
        <h2 class="text-white mb-4">Track Details</h2>
        <div class="card bg-dark text-white mb-4">
          <div class="card-header d-flex justify-content-between align-items-center">
            <h3 class="card-title">{{ t.name }}</h3>
            <a routerLink="/admin/courses/new" [queryParams]="{ trackId: t.id }" class="btn btn-success px-4">
              <i class="fas fa-plus me-2"></i> New Course
            </a>
          </div>
          <div class="card-body">
            <dl class="row">
              <dt class="col-sm-3">Track ID:</dt><dd class="col-sm-9">{{ t.id }}</dd>
              <dt class="col-sm-3">Description:</dt><dd class="col-sm-9">{{ t.description }}</dd>
              <dt class="col-sm-3">Number of Courses:</dt><dd class="col-sm-9">{{ t.courses.length }}</dd>
            </dl>
          </div>
        </div>

        <h3 class="text-white mb-3">Courses in this Track</h3>
        <div class="row g-4">
          @for (course of t.courses; track course.id) {
            <div class="col-lg-5 col-md-6">
              <div class="card bg-dark text-white">
                <img [src]="course.cover | fileUrl" alt="Course Cover" class="card-img-top" [routerLink]="['/courses', course.id]" />
                <div class="card-body">
                  <dl class="row">
                    <dt class="col-sm-4">Name:</dt><dd class="col-sm-8">{{ course.name }}</dd>
                    <dt class="col-sm-4">Instructor:</dt><dd class="col-sm-8">{{ course.instructorName }}</dd>
                    <dt class="col-sm-4">Price:</dt><dd class="col-sm-8">{{ course.price | currency }}</dd>
                    <dt class="col-sm-4">Description:</dt><dd class="col-sm-8">{{ course.description }}</dd>
                  </dl>
                </div>
              </div>
            </div>
          } @empty {
            <p class="text-muted">No courses are currently available in this track.</p>
          }
        </div>
      </div>
    }
  `,
  styles: `
    .card { border-radius: 10px; overflow: hidden; }
    .card-img-top { height: 200px; object-fit: cover; cursor: pointer; }
  `,
})
export class TrackDetailsPageComponent {
  private readonly api = inject(TracksApi);
  readonly id = input.required<string>();
  readonly track = signal<TrackDetails | null>(null);

  constructor() {
    effect(() => this.api.details(+this.id()).subscribe((t) => this.track.set(t)));
  }
}
