import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';
import { UserMenuComponent } from '../shared/user-menu.component';

/**
 * Instructor area layout (legacy _InstructorLayout).
 * The legacy Grades / Reports / Settings links pointed to actions that never existed,
 * so they are not shown (listed in the backlog).
 */
@Component({
  selector: 'app-instructor-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, UserMenuComponent],
  templateUrl: './instructor-layout.component.html',
})
export class InstructorLayoutComponent {
  readonly auth = inject(AuthService);

  readonly nav = [
    { label: 'Dashboard', icon: 'fas fa-tachometer-alt', link: '/instructor' },
    { label: 'Manage Exams', icon: 'fas fa-file-alt', link: '/instructor/exams' },
    { label: 'Courses', icon: 'fas fa-book', link: '/instructor/courses' },
    { label: 'Assignments', icon: 'fas fa-tasks', link: '/instructor/assignments' },
    { label: 'Students', icon: 'fas fa-users', link: '/instructor/students' },
  ];
}
