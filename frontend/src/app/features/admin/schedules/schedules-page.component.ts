import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ScheduleRow, SchedulesApi } from './schedules.api';

/** Legacy Admin/Schedule/Index: click a row to edit it. */
@Component({
  selector: 'app-schedules-page',
  imports: [RouterLink, DatePipe],
  styleUrl: '../shared/admin-table.scss',
  template: `
    <div class="container">
      <h2 class="text-center my-4 text-white">Class Schedule</h2>
      <a routerLink="/admin/schedules/new" class="btn new-course-btn mb-3"><i class="fas fa-plus"></i> New Schedual</a>
      <table class="table table-bordered schedule-table">
        <thead><tr><th>Time</th><th>Day</th><th>Course</th><th>Instructor</th><th>Room</th></tr></thead>
        <tbody>
          @for (s of schedules(); track s.id) {
            <tr [routerLink]="['/admin/schedules', s.id]" style="cursor: pointer">
              <td>{{ s.startTime | date: 'hh:mm a' }} - {{ s.endTime | date: 'hh:mm a' }}</td>
              <td>{{ s.dayOfWeek }}</td>
              <td>{{ s.courseName }}</td>
              <td>{{ s.instructorName }}</td>
              <td>{{ s.roomName }}</td>
            </tr>
          }
        </tbody>
      </table>
    </div>
  `,
})
export class SchedulesPageComponent {
  readonly schedules = signal<ScheduleRow[]>([]);

  constructor() {
    inject(SchedulesApi).getAll().subscribe((s) => this.schedules.set(s));
  }
}
