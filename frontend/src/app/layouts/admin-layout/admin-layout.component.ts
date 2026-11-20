import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { UserMenuComponent } from '../shared/user-menu.component';

interface NavItem {
  label: string;
  icon: string;
  link: string;
  query?: Record<string, string>;
}

/** Admin area layout (legacy Views/Shared/_Layout.cshtml). */
@Component({
  selector: 'app-admin-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive, UserMenuComponent],
  templateUrl: './admin-layout.component.html',
})
export class AdminLayoutComponent {
  readonly nav: NavItem[] = [
    { label: 'Dashboard', icon: 'fas fa-tachometer-alt', link: '/admin' },
    { label: 'Manage Students', icon: 'fas fa-user-graduate', link: '/admin/users', query: { filter: 'Student' } },
    { label: 'Manage Instructors', icon: 'fas fa-user-graduate', link: '/admin/users', query: { filter: 'Instructor' } },
    { label: 'Manage Trackes', icon: 'fas fa-chalkboard-teacher', link: '/admin/tracks' },
    { label: 'Manage Courses', icon: 'fas fa-chalkboard-teacher', link: '/admin/courses' },
    { label: 'Manage Departments', icon: 'fas fa-building', link: '/admin/departments' },
    { label: 'EnrollMents', icon: 'fas fa-calendar-alt', link: '/admin/enrollments' },
    { label: 'Class Schedules', icon: 'fas fa-calendar-alt', link: '/admin/schedules' },
    { label: 'Years', icon: 'fas fa-calendar-alt', link: '/admin/academic-years' },
  ];
}
