import { CourseCard } from '../../public/data-access/catalog.models';

export interface StudentDashboard {
  courses: { id: number; name: string; startDate: string | null; endDate: string | null }[];
  schedule: { courseId: number; courseName: string; startTime: string }[];
  feedbacks: { id: number; providerName: string | null; providerPicture: string | null; date: string; rating: number | null; content: string }[];
  recommendations: CourseCard[];
  posts: { id: number; content: string; attachment: string | null; createdAt: string; userName: string | null; userPicture: string | null; communityId: number | null }[];
  communities: { id: number; name: string; description: string }[];
}

export interface MyCourse {
  id: number;
  name: string;
  cover: string | null;
  instructorName: string | null;
  rate: number | null;
  progressPercentage: number;
}

export interface CourseFeedback {
  id: number;
  userName: string | null;
  userPicture: string | null;
  date: string;
  rating: number | null;
  content: string;
  isMine: boolean;
}

export interface CoursePlayer {
  courseId: number;
  courseName: string;
  sections: { id: number; title: string }[];
  feedbacks: CourseFeedback[];
}

export interface PlayerVideo { id: number; title: string; url: string }
export interface PlayerResource { id: number; title: string; url: string }
export interface PlayerAssignment { id: number; title: string; url: string; deadLine: string; submitted: boolean }

export interface SectionContent {
  id: number;
  title: string;
  content: string;
  videos: PlayerVideo[];
  resources: PlayerResource[];
  assignments: PlayerAssignment[];
  feedbacks: CourseFeedback[];
  hasQuiz: boolean;
}

export interface QuizQuestion {
  questionText: string;
  choices: string[];
}

export interface QuickAccess {
  courseId: number;
  courseName: string;
  sections: { title: string; content: string; videos: PlayerVideo[]; resources: PlayerResource[]; assignments: PlayerAssignment[] }[];
}

export interface FeedbackRequest {
  courseId: number | null;
  courseCurriculumId: number | null;
  rating: number | null;
  content: string;
}
