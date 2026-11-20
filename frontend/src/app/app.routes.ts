import { Routes } from '@angular/router';
import { authGuard, guestGuard, roleGuard } from './core/auth/auth.guards';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { CommunityLayoutComponent } from './layouts/community-layout/community-layout.component';
import { InstructorLayoutComponent } from './layouts/instructor-layout/instructor-layout.component';
import { PublicLayoutComponent } from './layouts/public-layout/public-layout.component';
import { StudentLayoutComponent } from './layouts/student-layout/student-layout.component';

/**
 * Top-level routes: each area = one layout + one lazy-loaded feature routes file.
 * To add a page, add it to the feature's *.routes.ts file (see docs/ADD-A-FEATURE.md).
 */
export const routes: Routes = [
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [roleGuard('Admin')],
    loadChildren: () => import('./features/admin/admin.routes').then((m) => m.ADMIN_ROUTES),
  },
  {
    path: 'instructor',
    component: InstructorLayoutComponent,
    canActivate: [roleGuard('Instructor')],
    loadChildren: () => import('./features/instructor/instructor.routes').then((m) => m.INSTRUCTOR_ROUTES),
  },
  {
    path: 'student',
    component: StudentLayoutComponent,
    canActivate: [roleGuard('Student')],
    loadChildren: () => import('./features/student/student.routes').then((m) => m.STUDENT_ROUTES),
  },
  {
    path: 'community',
    component: CommunityLayoutComponent,
    canActivate: [authGuard],
    loadChildren: () => import('./features/community/community.routes').then((m) => m.COMMUNITY_ROUTES),
  },
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      {
        path: 'auth',
        canActivate: [guestGuard],
        loadChildren: () => import('./features/auth/auth.routes').then((m) => m.AUTH_ROUTES),
      },
      {
        path: 'account',
        canActivate: [authGuard],
        loadChildren: () => import('./features/account/account.routes').then((m) => m.ACCOUNT_ROUTES),
      },
      {
        path: 'cart',
        canActivate: [authGuard],
        loadChildren: () => import('./features/cart/cart.routes').then((m) => m.CART_ROUTES),
      },
      {
        path: '',
        loadChildren: () => import('./features/public/public.routes').then((m) => m.PUBLIC_ROUTES),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
