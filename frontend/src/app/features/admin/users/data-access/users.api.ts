import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../../../core/api/base-api.service';
import { UserDetails, UserListItem, UserRequest } from '../models/user';

@Injectable({ providedIn: 'root' })
export class UsersApi extends BaseApiService<UserListItem, UserRequest, string, UserDetails> {
  protected readonly resource = 'admin/users';

  roles(): Observable<string[]> {
    return this.api.get<string[]>(`${this.resource}/roles`);
  }

  lock(id: string): Observable<void> {
    return this.api.post<void>(`${this.resource}/${id}/lock`);
  }

  unlock(id: string): Observable<void> {
    return this.api.post<void>(`${this.resource}/${id}/unlock`);
  }
}
