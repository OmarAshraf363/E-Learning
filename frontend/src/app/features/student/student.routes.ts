import { Routes } from '@angular/router';

/** Student area (legacy Areas/Student). */
export const STUDENT_ROUTES: Routes = [
  { path: '', loadComponent: () => import('./pages/dashboard-page.component').then((m) => m.DashboardPageComponent) },
  { path: 'courses', loadComponent: () => import('./pages/my-courses-page.component').then((m) => m.MyCoursesPageComponent) },
  { path: 'courses/:id', loadComponent: () => import('./pages/course-player-page.component').then((m) => m.CoursePlayerPageComponent) },
];
