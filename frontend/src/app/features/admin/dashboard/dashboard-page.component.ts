import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { ApiClient } from '../../../core/api/api-client';
import { PagedResult } from '../../../core/models/paged-result';
import { PaginationComponent } from '../../../shared/components/pagination.component';

interface AdminDashboard {
  totalStudents: number;
  totalInstructors: number;
  totalCourses: number;
  totalDepartments: number;
  courses: PagedResult<{ id: number; name: string; departmentName: string | null }>;
  mostPopularCourse: { name: string; enrollmentCount: number } | null;
  events: { name: string; date: string }[];
}

/** Legacy Admin/DashBoard/Index. */
@Component({
  selector: 'app-dashboard-page',
  imports: [DatePipe, PaginationComponent],
  templateUrl: './dashboard-page.component.html',
  styles: `
    table tr { color: #ccc; }
    .card { box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2); border-radius: 10px; }
    h3, h4 { color: #eee; }
  `,
})
export class DashboardPageComponent {
  private readonly api = inject(ApiClient);
  readonly data = signal<AdminDashboard | null>(null);

  constructor() {
    this.load(1);
  }

  load(page: number): void {
    this.api.get<AdminDashboard>('admin/dashboard', { page, pageSize: 6 }).subscribe((d) => this.data.set(d));
  }
}
