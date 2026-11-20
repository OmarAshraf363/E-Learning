import { Routes } from '@angular/router';

/** Admin area (legacy Areas/Admin). Every page is lazy loaded. */
export const ADMIN_ROUTES: Routes = [
  { path: '', loadComponent: () => import('./dashboard/dashboard-page.component').then((m) => m.DashboardPageComponent) },

  // Users (Phase 1)
  { path: 'users', loadComponent: () => import('./users/pages/users-page.component').then((m) => m.UsersPageComponent) },
  { path: 'users/new', loadComponent: () => import('./users/pages/user-upsert-page.component').then((m) => m.UserUpsertPageComponent) },
  { path: 'users/:id', loadComponent: () => import('./users/pages/user-upsert-page.component').then((m) => m.UserUpsertPageComponent) },

  // Departments
  { path: 'departments', loadComponent: () => import('./departments/pages/departments-page.component').then((m) => m.DepartmentsPageComponent) },
  { path: 'departments/:id', loadComponent: () => import('./departments/pages/department-details-page.component').then((m) => m.DepartmentDetailsPageComponent) },
  { path: 'departments/:id/courses', loadComponent: () => import('./departments/pages/department-courses-page.component').then((m) => m.DepartmentCoursesPageComponent) },

  // Tracks
  { path: 'tracks', loadComponent: () => import('./tracks/pages/tracks-page.component').then((m) => m.TracksPageComponent) },
  { path: 'tracks/:id', loadComponent: () => import('./tracks/pages/track-details-page.component').then((m) => m.TrackDetailsPageComponent) },

  // Courses
  { path: 'courses', loadComponent: () => import('./courses/pages/courses-page.component').then((m) => m.CoursesPageComponent) },
  { path: 'courses/new', loadComponent: () => import('./courses/pages/course-upsert-page.component').then((m) => m.CourseUpsertPageComponent) },
  { path: 'courses/:id', loadComponent: () => import('./courses/pages/course-upsert-page.component').then((m) => m.CourseUpsertPageComponent) },
  { path: 'courses/:id/details', loadComponent: () => import('./courses/pages/course-details-page.component').then((m) => m.CourseDetailsPageComponent) },

  // Academic years, schedules, enrollments
  { path: 'academic-years', loadComponent: () => import('./academic-years/academic-years-page.component').then((m) => m.AcademicYearsPageComponent) },
  { path: 'schedules', loadComponent: () => import('./schedules/schedules-page.component').then((m) => m.SchedulesPageComponent) },
  { path: 'schedules/new', loadComponent: () => import('./schedules/schedule-upsert-page.component').then((m) => m.ScheduleUpsertPageComponent) },
  { path: 'schedules/:id', loadComponent: () => import('./schedules/schedule-upsert-page.component').then((m) => m.ScheduleUpsertPageComponent) },
  { path: 'enrollments', loadComponent: () => import('./enrollments/enrollments-page.component').then((m) => m.EnrollmentsPageComponent) },
];
