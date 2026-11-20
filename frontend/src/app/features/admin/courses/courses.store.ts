import { inject, Injectable } from '@angular/core';
import { CrudStore } from '../../../core/state/crud-store';
import { AdminCourseRow, CourseForm, CourseRequest } from './course.models';
import { CoursesApi } from './courses.api';

@Injectable({ providedIn: 'root' })
export class CoursesStore extends CrudStore<AdminCourseRow, CourseRequest, number, CourseForm> {
  constructor() {
    super(inject(CoursesApi), (c) => c.id);
  }
}
