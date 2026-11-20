import { Routes } from '@angular/router';
import { ComingSoonComponent } from '../../shared/components/coming-soon.component';

/** Community feed. Migrated in Phase 7. */
export const COMMUNITY_ROUTES: Routes = [{ path: '', component: ComingSoonComponent, data: { title: 'Community', phase: 'Phase 7' } }];
