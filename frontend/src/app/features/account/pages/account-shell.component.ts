import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

/** Legacy Identity/Account/Manage layout: side nav + selected page. */
@Component({
  selector: 'app-account-shell',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div class="container py-4">
      <h1>Manage your account</h1>
      <hr />
      <div class="row">
        <div class="col-md-3">
          <ul class="nav nav-pills flex-column">
            <li class="nav-item">
              <a class="nav-link" routerLink="/account" routerLinkActive="active" [routerLinkActiveOptions]="{ exact: true }">Profile</a>
            </li>
            <li class="nav-item">
              <a class="nav-link" routerLink="/account/password" routerLinkActive="active">Password</a>
            </li>
          </ul>
        </div>
        <div class="col-md-9">
          <router-outlet />
        </div>
      </div>
    </div>
  `,
})
export class AccountShellComponent {}
