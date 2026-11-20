/** Values of the course form (GET /admin/courses/:id and /instructor/courses/:id). */
export interface CourseFormValue {
  id: number;
  courseName: string;
  description: string;
  credits: number;
  price: number | null;
  rate: number | null;
  demoVideoUrl: string | null;
  numOfEnrollments: number | null;
  startDate: string | null;
  endDate: string | null;
  instructorId: string;
  departmentId: number | null;
  trackId: number | null;
  imgCover: string | null;
  keyWords: string[];
  learningObjectives: string[];
  topicsCovered: string[];
}

/** multipart body (legacy CourseVM). Cover is required on create. */
export interface CourseRequest {
  courseName: string;
  description: string;
  credits: number | null;
  price: number | null;
  rate: number | null;
  demoVideoUrl: string | null;
  numOfEnrollments: number | null;
  startDate: string | null;
  endDate: string | null;
  instructorId: string | null;
  departmentId: number | null;
  trackId: number | null;
  imgCover: File | null;
  keyWords: string[];
  learningObjectives: string[];
  topicsCovered: string[];
}

export interface Option<TId = number> {
  id: TId;
  name: string;
}
