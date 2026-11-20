import { Routes } from '@angular/router';

/** Instructor area (legacy Areas/Instructor). */
export const INSTRUCTOR_ROUTES: Routes = [
  { path: '', loadComponent: () => import('./pages/dashboard-page.component').then((m) => m.DashboardPageComponent) },

  { path: 'courses', loadComponent: () => import('./pages/courses-page.component').then((m) => m.CoursesPageComponent) },
  { path: 'courses/new', loadComponent: () => import('./pages/course-form-page.component').then((m) => m.CourseFormPageComponent) },
  { path: 'courses/:id', loadComponent: () => import('./pages/course-form-page.component').then((m) => m.CourseFormPageComponent) },
  { path: 'courses/:id/curriculum', loadComponent: () => import('./pages/curriculum-page.component').then((m) => m.CurriculumPageComponent) },

  { path: 'exams', loadComponent: () => import('./pages/exams-page.component').then((m) => m.ExamsPageComponent) },
  { path: 'exams/new', loadComponent: () => import('./pages/exam-form-page.component').then((m) => m.ExamFormPageComponent) },
  { path: 'exams/:id', loadComponent: () => import('./pages/exam-form-page.component').then((m) => m.ExamFormPageComponent) },
  { path: 'exams/:id/questions', loadComponent: () => import('./pages/exam-questions-page.component').then((m) => m.ExamQuestionsPageComponent) },
  { path: 'exams/:id/questions/new', loadComponent: () => import('./pages/question-form-page.component').then((m) => m.QuestionFormPageComponent) },
  { path: 'exams/:id/questions/:questionId', loadComponent: () => import('./pages/question-form-page.component').then((m) => m.QuestionFormPageComponent) },

  { path: 'assignments', loadComponent: () => import('./pages/assignments-page.component').then((m) => m.AssignmentsPageComponent) },
  { path: 'students', loadComponent: () => import('./pages/students-page.component').then((m) => m.StudentsPageComponent) },
];
