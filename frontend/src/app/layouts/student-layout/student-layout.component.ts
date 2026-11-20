import { Component } from '@angular/core';
import { RouterLink, RouterOutlet } from '@angular/router';
import { UserMenuComponent } from '../shared/user-menu.component';

/** Student area layout (legacy _StudentLayout). */
@Component({
  selector: 'app-student-layout',
  imports: [RouterOutlet, RouterLink, UserMenuComponent],
  templateUrl: './student-layout.component.html',
})
export class StudentLayoutComponent {}
