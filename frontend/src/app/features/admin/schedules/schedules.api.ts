import { Injectable } from '@angular/core';
import { BaseApiService } from '../../../core/api/base-api.service';
import { ScheduleRow } from '../departments/department.models';

export type { ScheduleRow };

/** Times are "HH:mm". */
export interface ScheduleForm {
  id: number;
  startTime: string;
  endTime: string;
  dayOfWeek: string;
  courseId: number;
  instructorId: string | null;
  roomId: number | null;
  departmentId: number | null;
  acadmicYearId: number | null;
}

export type ScheduleRequest = Omit<ScheduleForm, 'id'>;

@Injectable({ providedIn: 'root' })
export class SchedulesApi extends BaseApiService<ScheduleRow, ScheduleRequest, number, ScheduleForm> {
  protected readonly resource = 'admin/schedules';
}
