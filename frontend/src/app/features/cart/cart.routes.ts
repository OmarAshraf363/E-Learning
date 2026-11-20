import { Routes } from '@angular/router';

/** Cart + Stripe checkout (legacy Customer/Cart, Customer/checkout). */
export const CART_ROUTES: Routes = [
  { path: '', loadComponent: () => import('./pages/cart-page.component').then((m) => m.CartPageComponent) },
  { path: 'checkout/success', loadComponent: () => import('./pages/checkout-success-page.component').then((m) => m.CheckoutSuccessPageComponent) },
  { path: 'checkout/cancel', loadComponent: () => import('./pages/checkout-cancel-page.component').then((m) => m.CheckoutCancelPageComponent) },
];
