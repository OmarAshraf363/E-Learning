import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../../core/api/base-api.service';
import { AdminCourseDetails, AdminCourseRow, CourseForm, CourseRequest } from './course.models';

@Injectable({ providedIn: 'root' })
export class CoursesApi extends BaseApiService<AdminCourseRow, CourseRequest, number, CourseForm> {
  protected readonly resource = 'admin/courses';
  protected override readonly useFormData = true;

  details(id: number): Observable<AdminCourseDetails> {
    return this.api.get<AdminCourseDetails>(`${this.resource}/${id}/details`);
  }
}
