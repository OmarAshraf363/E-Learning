import { inject, Injectable } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { CrudStore } from '../../../../core/state/crud-store';
import { UserDetails, UserListItem, UserRequest } from '../models/user';
import { UsersApi } from './users.api';

@Injectable({ providedIn: 'root' })
export class UsersStore extends CrudStore<UserListItem, UserRequest, string, UserDetails> {
  private readonly usersApi: UsersApi;

  constructor() {
    const api = inject(UsersApi);
    super(api, (u) => u.id);
    this.usersApi = api;
  }

  async setLocked(user: UserListItem, locked: boolean): Promise<void> {
    await this.run(() => firstValueFrom(locked ? this.usersApi.lock(user.id) : this.usersApi.unlock(user.id)));
    this.items.update((list) => list.map((u) => (u.id === user.id ? { ...u, isLockedOut: locked } : u)));
  }
}
