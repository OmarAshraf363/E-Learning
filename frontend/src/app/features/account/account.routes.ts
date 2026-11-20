import { Routes } from '@angular/router';
import { ComingSoonComponent } from '../../shared/components/coming-soon.component';

/** Profile / change password (legacy Identity/Account/Manage). Migrated in Phase 1. */
export const ACCOUNT_ROUTES: Routes = [{ path: '', component: ComingSoonComponent, data: { title: 'My account', phase: 'Phase 1' } }];
