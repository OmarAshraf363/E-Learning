import { Routes } from '@angular/router';
import { ComingSoonComponent } from '../../shared/components/coming-soon.component';

/** Instructor area. Migrated in Phase 5. */
export const INSTRUCTOR_ROUTES: Routes = [
  { path: '', component: ComingSoonComponent, data: { title: 'Dashboard', phase: 'Phase 5' } },
  { path: 'exams', component: ComingSoonComponent, data: { title: 'Exams', phase: 'Phase 5' } },
  { path: 'courses', component: ComingSoonComponent, data: { title: 'Courses', phase: 'Phase 5' } },
  { path: 'assignments', component: ComingSoonComponent, data: { title: 'Assignments', phase: 'Phase 5' } },
  { path: 'students', component: ComingSoonComponent, data: { title: 'Students', phase: 'Phase 5' } },
];
