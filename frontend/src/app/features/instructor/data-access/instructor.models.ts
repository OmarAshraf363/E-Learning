export interface InstructorDashboard {
  totalCourses: number;
  totalExams: number;
  totalQuestions: number;
  totalStudents: number;
}

export interface InstructorCourseRow {
  id: number;
  name: string;
  studentsEnrolled: number;
}

export interface Option<TId = number> {
  id: TId;
  name: string;
}

export interface CurriculumVideo { id: number; title: string; url: string }
export interface CurriculumReference { id: number; title: string; url: string }
export interface CurriculumAssignment { id: number; title: string; description: string; deadLine: string; content: string }
export interface CurriculumExam { id: number; title: string; examDate: string }

export interface CurriculumSection {
  id: number;
  title: string;
  content: string;
  videos: CurriculumVideo[];
  references: CurriculumReference[];
  assignments: CurriculumAssignment[];
  exams: CurriculumExam[];
}

export interface CourseCurriculum {
  courseId: number;
  courseName: string;
  sections: CurriculumSection[];
}

export interface ExamRow { id: number; title: string; courseName: string | null; examDate: string; totalQuestions: number }
export interface Exam { id: number; title: string; examDate: string; courseId: number }
export interface Choice { id?: number; choiceText: string; isCorrect: boolean }
export interface Question { id: number; questionText: string; examId: number; choices: Choice[] }
export interface ExamQuestions { examId: number; examTitle: string; questions: Question[] }

export interface Assignment { id: number; curriculumId: number; title: string; description: string; deadLine: string; content: string }
export interface SectionAssignments { id: number; title: string; assignments: Assignment[] }
export interface Submission { id: number; studentName: string | null; submissionDate: string; content: string; grade: number | null; feedback: string }

export interface CourseStudent { id: string; fullName: string; email: string | null; picture: string | null; gpa: number | null }
