import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../../../core/api/api-client';

export interface Profile {
  userName: string;
  phoneNumber: string | null;
  jopDescription: string;
  picture: string | null;
}

export interface UpdateProfileRequest {
  phoneNumber: string | null;
  jopDescription: string | null;
  profilePicture: File | null;
}

export interface ChangePasswordRequest {
  oldPassword: string;
  newPassword: string;
  confirmPassword: string;
}

@Injectable({ providedIn: 'root' })
export class AccountApi {
  private readonly api = inject(ApiClient);

  getProfile(): Observable<Profile> {
    return this.api.get<Profile>('account/profile');
  }

  updateProfile(body: UpdateProfileRequest): Observable<Profile> {
    return this.api.putForm<Profile>('account/profile', body);
  }

  changePassword(body: ChangePasswordRequest): Observable<void> {
    return this.api.put<void>('account/password', body);
  }
}
