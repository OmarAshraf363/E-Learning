import { Routes } from '@angular/router';
import { ComingSoonComponent } from '../../shared/components/coming-soon.component';

/** Admin area. Users in Phase 1, the rest in Phase 2. */
export const ADMIN_ROUTES: Routes = [
  { path: '', component: ComingSoonComponent, data: { title: 'Dashboard', phase: 'Phase 2' } },
  { path: 'users', component: ComingSoonComponent, data: { title: 'Users', phase: 'Phase 1' } },
  { path: 'tracks', component: ComingSoonComponent, data: { title: 'Tracks', phase: 'Phase 2' } },
  { path: 'courses', component: ComingSoonComponent, data: { title: 'Courses', phase: 'Phase 2' } },
  { path: 'departments', component: ComingSoonComponent, data: { title: 'Departments', phase: 'Phase 2' } },
  { path: 'enrollments', component: ComingSoonComponent, data: { title: 'Enrollments', phase: 'Phase 2' } },
  { path: 'schedules', component: ComingSoonComponent, data: { title: 'Class schedules', phase: 'Phase 2' } },
  { path: 'academic-years', component: ComingSoonComponent, data: { title: 'Academic years', phase: 'Phase 2' } },
];
