import { inject, Injectable } from '@angular/core';
import { CrudStore } from '../../../core/state/crud-store';
import { DepartmentsApi } from './departments.api';
import { Department, DepartmentListItem, DepartmentRequest } from './department.models';

@Injectable({ providedIn: 'root' })
export class DepartmentsStore extends CrudStore<DepartmentListItem, DepartmentRequest, number, Department> {
  constructor() {
    super(inject(DepartmentsApi), (d) => d.id);
  }
}
