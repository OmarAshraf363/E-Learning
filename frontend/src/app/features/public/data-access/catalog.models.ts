import { PagedResult } from '../../../core/models/paged-result';

/** Course card + hover popover. */
export interface CourseCard {
  id: number;
  title: string;
  image: string | null;
  instructorName: string | null;
  rate: number | null;
  price: number | null;
  description: string;
  numOfEnrolments: number;
  videosCount: number;
  learningObjectives: string[];
}

export interface DepartmentCard {
  id: number;
  name: string;
  description: string;
  cover: string | null;
}

export interface TrackCard {
  id: number;
  title: string;
  description: string;
  cover: string | null;
  numOfCourses: number;
  numOfEnrollments: number;
}

export interface Home {
  topCourses: CourseCard[];
  topDepartments: DepartmentCard[];
  topTracks: TrackCard[];
  courses: PagedResult<CourseCard>;
  recommendations: CourseCard[];
  enrolledCourseIds: number[];
  testimonials: { id: number; userName: string | null; userPicture: string | null; courseId: number; courseName: string; content: string; date: string }[];
  events: { id: number; date: string; name: string; description: string; createdBy: string | null; department: string | null }[];
}

export interface DepartmentCourses {
  departmentName: string | null;
  departmentDescription: string | null;
  courses: PagedResult<CourseCard>;
}

/** Courses page filters (legacy CourseParam). */
export interface CourseFilter {
  search?: string | null;
  instructorId?: string | null;
  categoryId?: number | null;
  price?: number | null;
  rate?: number | null;
  sortBy?: string | null;
  sortOrder?: string | null;
}

export interface CourseDetails {
  id: number;
  courseName: string;
  description: string;
  demoVideoUrl: string | null;
  demoTitle: string | null;
  rate: number | null;
  price: number | null;
  numOfFeedbacks: number;
  numOfVideos: number;
  numOfEnrollments: number;
  instructorId: string;
  instructorName: string | null;
  instructorBio: string | null;
  instructorImage: string | null;
  learningObjectives: string[];
  topicsCovered: string[];
  relatedCourses: CourseCard[];
  instructorCourses: CourseCard[];
  reviewSummary: { totalReviews: number; finalRate: number; countByStars: Record<string, number> };
  reviews: { id: number; content: string; createdAt: string; rating: number | null; userName: string; userImageUrl: string | null }[];
}

export interface Category {
  id: number;
  name: string;
  description: string;
  courses: CourseCard[];
  keyWords: string[];
}

export interface PublicTrack {
  id: number;
  title: string;
  description: string;
  cover: string | null;
  numOfCourses: number;
  numOfEnrollments: number;
  numOfCoursesStudentHas: number;
  courses: CourseCard[];
  coursesStudentDoesNotHave: CourseCard[];
}

export interface NavCategory {
  id: number;
  name: string;
  courses: { id: number; name: string }[];
}
