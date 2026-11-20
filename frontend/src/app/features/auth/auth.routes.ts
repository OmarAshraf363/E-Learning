import { Routes } from '@angular/router';
import { ComingSoonComponent } from '../../shared/components/coming-soon.component';

/** Login / register (legacy Identity pages). Migrated in Phase 1. */
export const AUTH_ROUTES: Routes = [
  { path: 'login', component: ComingSoonComponent, data: { title: 'Login', phase: 'Phase 1' } },
  { path: 'register', component: ComingSoonComponent, data: { title: 'Register', phase: 'Phase 1' } },
];
