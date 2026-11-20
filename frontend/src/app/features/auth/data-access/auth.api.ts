import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../../../core/api/api-client';

export interface RegisterInstructorRequest {
  fullName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

/** Auth endpoints that are not part of the session (login/register live in core AuthService). */
@Injectable({ providedIn: 'root' })
export class AuthApi {
  private readonly api = inject(ApiClient);

  /** "Teach in BFCAI": creates an Instructor account (no sign in). */
  registerInstructor(body: RegisterInstructorRequest): Observable<void> {
    return this.api.post<void>('auth/register-instructor', body);
  }
}
