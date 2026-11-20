import { Routes } from '@angular/router';
import { ComingSoonComponent } from '../../shared/components/coming-soon.component';

/** Admin area (legacy Areas/Admin). */
export const ADMIN_ROUTES: Routes = [
  { path: '', component: ComingSoonComponent, data: { title: 'Dashboard', phase: 'Phase 2' } },
  { path: 'users', loadComponent: () => import('./users/pages/users-page.component').then((m) => m.UsersPageComponent) },
  {
    path: 'users/new',
    loadComponent: () => import('./users/pages/user-upsert-page.component').then((m) => m.UserUpsertPageComponent),
  },
  {
    path: 'users/:id',
    loadComponent: () => import('./users/pages/user-upsert-page.component').then((m) => m.UserUpsertPageComponent),
  },
  { path: 'tracks', component: ComingSoonComponent, data: { title: 'Tracks', phase: 'Phase 2' } },
  { path: 'courses', component: ComingSoonComponent, data: { title: 'Courses', phase: 'Phase 2' } },
  { path: 'departments', component: ComingSoonComponent, data: { title: 'Departments', phase: 'Phase 2' } },
  { path: 'enrollments', component: ComingSoonComponent, data: { title: 'Enrollments', phase: 'Phase 2' } },
  { path: 'schedules', component: ComingSoonComponent, data: { title: 'Class schedules', phase: 'Phase 2' } },
  { path: 'academic-years', component: ComingSoonComponent, data: { title: 'Academic years', phase: 'Phase 2' } },
];
