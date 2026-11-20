import { Routes } from '@angular/router';

/** Login / register (legacy Identity pages). */
export const AUTH_ROUTES: Routes = [
  { path: 'login', loadComponent: () => import('./pages/login/login-page.component').then((m) => m.LoginPageComponent) },
  {
    path: 'register',
    loadComponent: () => import('./pages/register/register-page.component').then((m) => m.RegisterPageComponent),
  },
  { path: '', pathMatch: 'full', redirectTo: 'login' },
];
