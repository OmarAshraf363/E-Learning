import { HttpErrorResponse, HttpInterceptorFn, HttpRequest } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, switchMap, throwError } from 'rxjs';
import { environment } from '../../../environments/environment';
import { AuthService } from './auth.service';
import { SKIP_AUTH_REFRESH } from './auth.tokens';

const withToken = (req: HttpRequest<unknown>, token: string | null) =>
  token ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } }) : req;

/**
 * 1. Adds "Authorization: Bearer <token>" to API requests.
 * 2. On 401, refreshes the token once and retries; if refresh fails the user is logged out.
 */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  if (!req.url.startsWith(environment.apiUrl)) return next(req);

  const auth = inject(AuthService);

  return next(withToken(req, auth.accessToken)).pipe(
    catchError((error: unknown) => {
      const canRefresh =
        error instanceof HttpErrorResponse &&
        error.status === 401 &&
        !req.context.get(SKIP_AUTH_REFRESH) &&
        auth.isLoggedIn();

      if (!canRefresh) return throwError(() => error);

      return auth.refresh().pipe(
        switchMap((session) => next(withToken(req, session.accessToken))),
        catchError((refreshError: unknown) => {
          auth.logout();
          return throwError(() => refreshError);
        }),
      );
    }),
  );
};
