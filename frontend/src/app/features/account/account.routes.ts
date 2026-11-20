import { Routes } from '@angular/router';
import { AccountShellComponent } from './pages/account-shell.component';

/** Profile / change password (legacy Identity/Account/Manage). */
export const ACCOUNT_ROUTES: Routes = [
  {
    path: '',
    component: AccountShellComponent,
    children: [
      { path: '', loadComponent: () => import('./pages/profile-page.component').then((m) => m.ProfilePageComponent) },
      {
        path: 'password',
        loadComponent: () => import('./pages/change-password-page.component').then((m) => m.ChangePasswordPageComponent),
      },
    ],
  },
];
