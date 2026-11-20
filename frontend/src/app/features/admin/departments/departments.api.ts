import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../../core/api/base-api.service';
import { AdminCourseRow, Department, DepartmentDetails, DepartmentListItem, DepartmentRequest } from './department.models';

@Injectable({ providedIn: 'root' })
export class DepartmentsApi extends BaseApiService<DepartmentListItem, DepartmentRequest, number, Department> {
  protected readonly resource = 'admin/departments';
  protected override readonly useFormData = true;

  details(id: number): Observable<DepartmentDetails> {
    return this.api.get<DepartmentDetails>(`${this.resource}/${id}/details`);
  }

  courses(id: number): Observable<AdminCourseRow[]> {
    return this.api.get<AdminCourseRow[]>(`${this.resource}/${id}/courses`);
  }
}
