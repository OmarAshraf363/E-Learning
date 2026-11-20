import { Routes } from '@angular/router';
import { ComingSoonComponent } from '../../shared/components/coming-soon.component';

/** Student area. Migrated in Phase 6. */
export const STUDENT_ROUTES: Routes = [
  { path: '', component: ComingSoonComponent, data: { title: 'Dashboard', phase: 'Phase 6' } },
  { path: 'courses', component: ComingSoonComponent, data: { title: 'My courses', phase: 'Phase 6' } },
  { path: 'courses/:id', component: ComingSoonComponent, data: { title: 'Course', phase: 'Phase 6' } },
];
