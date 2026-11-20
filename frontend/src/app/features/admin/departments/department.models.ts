export interface DepartmentListItem {
  id: number;
  name: string;
  description: string;
  cover: string | null;
  totalHours: number;
}

export interface Department {
  id: number;
  name: string;
  description: string;
  cover: string | null;
}

/** multipart body; cover is required on create. */
export interface DepartmentRequest {
  departmentName: string;
  departmentDescription: string;
  cover: File | null;
}

export interface ScheduleRow {
  id: number;
  startTime: string;
  endTime: string;
  dayOfWeek: string;
  courseName: string;
  instructorName: string | null;
  roomName: string | null;
}

export interface DepartmentDetails {
  id: number;
  name: string;
  totalStudents: number;
  totalInstructors: number;
  totalHours: number;
  totalCourses: number;
  courses: { id: number; name: string; credits: number }[];
  instructors: { id: string; userName: string; email: string }[];
  schedules: ScheduleRow[];
  events: { id: number; name: string; date: string }[];
}

/** Row of the admin courses tables. */
export interface AdminCourseRow {
  id: number;
  name: string;
  departmentName: string | null;
  instructorName: string | null;
}
