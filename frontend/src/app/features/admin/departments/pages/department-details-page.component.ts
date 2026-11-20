import { DatePipe } from '@angular/common';
import { Component, effect, inject, input, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from '../../../../core/ui/notify.service';
import { DepartmentFormModalComponent } from '../components/department-form-modal.component';
import { DepartmentDetails } from '../department.models';
import { DepartmentsApi } from '../departments.api';
import { DepartmentsStore } from '../departments.store';
import { firstValueFrom } from 'rxjs';
import { CoursesApi } from '../../courses/courses.api';

/** Legacy Admin/Department/Details. Route: departments/:id */
@Component({
  selector: 'app-department-details-page',
  imports: [RouterLink, DatePipe],
  templateUrl: './department-details-page.component.html',
  styles: `
    h4 { color: lightgray; }
    .info-box { background-color: #333; color: #f8f9fa; padding: 15px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,.1); }
    .info-box h5 { margin-bottom: 10px; font-size: 16px; }
    .info-box p { font-size: 14px; }
    .schedule-table tr { cursor: pointer; }
  `,
})
export class DepartmentDetailsPageComponent {
  private readonly api = inject(DepartmentsApi);
  private readonly store = inject(DepartmentsStore);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);
  private readonly router = inject(Router);
  private readonly coursesApi = inject(CoursesApi);

  readonly id = input.required<string>();
  readonly details = signal<DepartmentDetails | null>(null);

  constructor() {
    effect(() => this.load());
  }

  load(): void {
    this.api.details(+this.id()).subscribe((d) => this.details.set(d));
  }

  edit(): void {
    const ref = this.modal.open(DepartmentFormModalComponent, { centered: true });
    (ref.componentInstance as DepartmentFormModalComponent).id = +this.id();
    ref.result.then(() => this.load(), () => undefined);
  }

  async remove(): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await this.store.remove(+this.id());
    void this.router.navigateByUrl('/admin/departments');
  }

  async removeCourse(courseId: number): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await firstValueFrom(this.coursesApi.delete(courseId));
    this.load();
  }
}
