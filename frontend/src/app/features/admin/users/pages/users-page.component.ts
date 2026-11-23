import { Component, effect, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../core/auth/auth.service';
import { NotifyService } from '../../../../core/ui/notify.service';
import { UsersStore } from '../data-access/users.store';
import { UserListItem } from '../models/user';

/** Legacy Admin/User/Index: students or instructors table (?filter=Student|Instructor). */
@Component({
  selector: 'app-users-page',
  imports: [RouterLink],
  templateUrl: './users-page.component.html',
})
export class UsersPageComponent {
  readonly store = inject(UsersStore);
  private readonly notify = inject(NotifyService);
  /** Business rule (second plan): an admin can't delete or lock their own account. */
  readonly myId = inject(AuthService).user()?.id;

  /** ?filter=Student | Instructor */
  readonly filter = input<string>();

  constructor() {
    effect(() => void this.store.load({ filter: this.filter() }));
  }

  async remove(user: UserListItem): Promise<void> {
    if (!(await this.notify.confirm('Are you sure you want to delete this user?'))) return;
    await this.store.remove(user.id);
    this.notify.success('User deleted.');
  }

  toggleLock(user: UserListItem): void {
    void this.store.setLocked(user, !user.isLockedOut);
  }
}
