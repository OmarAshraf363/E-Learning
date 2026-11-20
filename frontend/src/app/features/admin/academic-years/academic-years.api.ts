import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/api/base-api.service';

export interface AcademicYear {
  id: number;
  yearName: string;
}

export interface AcademicYearRequest {
  yearName: string;
}

@Injectable({ providedIn: 'root' })
export class AcademicYearsApi extends BaseApiService<AcademicYear, AcademicYearRequest> {
  protected readonly resource = 'admin/academic-years';
}
