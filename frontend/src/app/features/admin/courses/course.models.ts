export type { CourseFormValue as CourseForm, CourseRequest } from '../../../shared/course-form/course-form.models';
export type { AdminCourseRow } from '../departments/department.models';

export interface AdminCourseDetails {
  id: number;
  name: string;
  description: string;
  credits: number;
  instructorName: string | null;
  departmentName: string | null;
  enrollmentsCount: number;
}


