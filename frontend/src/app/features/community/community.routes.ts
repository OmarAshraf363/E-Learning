import { Routes } from '@angular/router';

/** Community feed (legacy Customer/Community). */
export const COMMUNITY_ROUTES: Routes = [
  { path: '', loadComponent: () => import('./pages/feed-page.component').then((m) => m.FeedPageComponent) },
];
