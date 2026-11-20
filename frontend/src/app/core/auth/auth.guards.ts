import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Role } from './auth.models';
import { AuthService } from './auth.service';

/** Route only for logged-in users; otherwise go to login (with returnUrl). */
export const authGuard: CanActivateFn = (_route, state) => {
  const auth = inject(AuthService);
  return auth.isLoggedIn()
    ? true
    : inject(Router).createUrlTree(['/auth/login'], { queryParams: { returnUrl: state.url } });
};

/**
 * Route only for the given roles. Usage in routes:
 *   { path: 'admin', canActivate: [roleGuard('Admin')], ... }
 */
export const roleGuard =
  (...roles: Role[]): CanActivateFn =>
  (_route, state) => {
    const auth = inject(AuthService);
    const router = inject(Router);
    if (!auth.isLoggedIn()) return router.createUrlTree(['/auth/login'], { queryParams: { returnUrl: state.url } });
    return auth.hasRole(...roles) ? true : router.createUrlTree([auth.homeUrl()]);
  };

/** Login / register pages: logged-in users are sent to their home page. */
export const guestGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  return auth.isLoggedIn() ? inject(Router).createUrlTree([auth.homeUrl()]) : true;
};
