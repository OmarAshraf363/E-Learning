import { Routes } from '@angular/router';
import { ComingSoonComponent } from '../../shared/components/coming-soon.component';

/** Public storefront (legacy Customer/Home). Migrated in Phase 3. */
export const PUBLIC_ROUTES: Routes = [
  { path: '', component: ComingSoonComponent, data: { title: 'Home', phase: 'Phase 3' } },
  { path: 'courses', component: ComingSoonComponent, data: { title: 'Courses', phase: 'Phase 3' } },
  { path: 'courses/:id', component: ComingSoonComponent, data: { title: 'Course details', phase: 'Phase 3' } },
  { path: 'category', component: ComingSoonComponent, data: { title: 'Categories', phase: 'Phase 3' } },
  { path: 'category/:id', component: ComingSoonComponent, data: { title: 'Category', phase: 'Phase 3' } },
  { path: 'topic/:word', component: ComingSoonComponent, data: { title: 'Topic courses', phase: 'Phase 3' } },
  { path: 'tracks/:id', component: ComingSoonComponent, data: { title: 'Track details', phase: 'Phase 3' } },
];
