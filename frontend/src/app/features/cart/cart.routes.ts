import { Routes } from '@angular/router';
import { ComingSoonComponent } from '../../shared/components/coming-soon.component';

/** Cart + Stripe checkout (legacy Customer/Cart, Customer/checkout). Migrated in Phase 4. */
export const CART_ROUTES: Routes = [
  { path: '', component: ComingSoonComponent, data: { title: 'Cart', phase: 'Phase 4' } },
  { path: 'checkout/success', component: ComingSoonComponent, data: { title: 'Payment successful', phase: 'Phase 4' } },
  { path: 'checkout/cancel', component: ComingSoonComponent, data: { title: 'Payment cancelled', phase: 'Phase 4' } },
];
