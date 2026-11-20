import { Routes } from '@angular/router';
import { homeRedirectGuard } from '../../core/auth/auth.guards';

/** Public storefront (legacy Customer/Home). */
export const PUBLIC_ROUTES: Routes = [
  { path: '', pathMatch: 'full', canActivate: [homeRedirectGuard], loadComponent: () => import('./pages/home-page.component').then((m) => m.HomePageComponent) },
  { path: 'courses', loadComponent: () => import('./pages/courses-page.component').then((m) => m.CoursesPageComponent) },
  { path: 'courses/:id', loadComponent: () => import('./pages/course-details-page.component').then((m) => m.CourseDetailsPageComponent) },
  { path: 'category', loadComponent: () => import('./pages/categories-page.component').then((m) => m.CategoriesPageComponent) },
  { path: 'category/:id', loadComponent: () => import('./pages/category-page.component').then((m) => m.CategoryPageComponent) },
  { path: 'topic/:word', loadComponent: () => import('./pages/topic-page.component').then((m) => m.TopicPageComponent) },
  { path: 'tracks/:id', loadComponent: () => import('./pages/track-page.component').then((m) => m.TrackPageComponent) },
];
