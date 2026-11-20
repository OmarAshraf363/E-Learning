import { HttpContextToken, HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ProblemDetails } from '../models/problem-details';
import { NotifyService } from '../ui/notify.service';

/** Set on a request when the caller handles errors itself (no automatic alert). */
export const SKIP_ERROR_ALERT = new HttpContextToken<boolean>(() => false);

/**
 * Shows an alert for failed API calls using the API's ProblemDetails.
 * - 400 with field errors: no alert; forms display them (see FormErrorsComponent / applyServerErrors).
 * - 401: handled by the auth interceptor.
 */
export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const notify = inject(NotifyService);

  return next(req).pipe(
    catchError((error: unknown) => {
      if (error instanceof HttpErrorResponse && !req.context.get(SKIP_ERROR_ALERT)) {
        const problem = (error.error ?? {}) as ProblemDetails;
        const hasFieldErrors = error.status === 400 && !!problem.errors;

        if (error.status === 0) notify.error('Cannot reach the server. Please try again.');
        else if (error.status !== 401 && !hasFieldErrors)
          notify.error(problem.detail ?? problem.title ?? 'Something went wrong.');
      }
      return throwError(() => error);
    }),
  );
};

/** Extracts field errors from a failed request ({} when there are none). */
export function fieldErrors(error: unknown): Record<string, string[]> {
  if (error instanceof HttpErrorResponse) return ((error.error ?? {}) as ProblemDetails).errors ?? {};
  return {};
}
