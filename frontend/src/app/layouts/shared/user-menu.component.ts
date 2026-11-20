import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';
import { FileUrlPipe } from '../../shared/pipes/file-url.pipe';

/** Port of the legacy _LoginPartial: avatar + logout, or Register / Login buttons. */
@Component({
  selector: 'app-user-menu',
  imports: [RouterLink, FileUrlPipe],
  template: `
    <ul class="d-flex list-unstyled justify-content-center align-items-center gap-3 m-0">
      @if (auth.user(); as user) {
        <li>
          <a routerLink="/account" [title]="user.fullName || user.userName">
            <img class="user-avatar" [src]="user.picture | fileUrl: defaultAvatar" alt="Profile" />
          </a>
        </li>
        <li>
          <button type="button" class="nav-link btn btn-link text-dark border-0" title="Logout" (click)="auth.logout()">
            <i class="fa-solid fa-arrow-right-from-bracket"></i>
          </button>
        </li>
      } @else {
        <li><a class="btn btn-dark" routerLink="/auth/register">Register</a></li>
        <li><a class="btn btn-light text-dark" routerLink="/auth/login">Login</a></li>
      }
    </ul>
  `,
  styles: `
    .user-avatar { width: 50px; height: 50px; border-radius: 50px; object-fit: cover; cursor: pointer; }
  `,
})
export class UserMenuComponent {
  readonly auth = inject(AuthService);
  readonly defaultAvatar = 'images/default-avatar.svg';
}
