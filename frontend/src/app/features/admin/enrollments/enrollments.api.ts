import { HttpContext } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../../../core/api/api-client';
import { SKIP_ERROR_ALERT } from '../../../core/errors/error.interceptor';

export interface EnrollmentOverview {
  totalEnrollments: number;
  latest: { id: number; studentEmail: string | null; courseName: string | null }[];
}

@Injectable({ providedIn: 'root' })
export class EnrollmentsApi {
  private readonly api = inject(ApiClient);

  overview(): Observable<EnrollmentOverview> {
    return this.api.get<EnrollmentOverview>('admin/enrollments');
  }

  /** The page shows the error message inline, so no global alert. */
  enrollByEmail(body: { email: string; courseId: number }): Observable<void> {
    return this.api.post<void>('admin/enrollments/by-email', body, {
      context: new HttpContext().set(SKIP_ERROR_ALERT, true),
    });
  }

  uploadCsv(csvFile: File): Observable<{ added: number; skipped: number; alreadyEnrolled: number }> {
    return this.api.postForm('admin/enrollments/csv', { csvFile });
  }
}
