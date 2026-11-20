import { DatePipe } from '@angular/common';
import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../core/auth/auth.service';
import { PagedResult } from '../../../core/models/paged-result';
import { PaginationComponent } from '../../../shared/components/pagination.component';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { InstructorRegisterModalComponent } from '../../auth/components/instructor-register-modal.component';
import { CourseGridComponent } from '../components/course-grid.component';
import { CatalogApi } from '../data-access/catalog.api';
import { CourseCard, Home } from '../data-access/catalog.models';

/**
 * Legacy Customer/Home/Index: banner, top courses, courses by category tabs (paged),
 * top categories, tracks, recommendations (students), testimonials and events.
 */
@Component({
  selector: 'app-home-page',
  imports: [RouterLink, DatePipe, FileUrlPipe, CourseGridComponent, PaginationComponent],
  templateUrl: './home-page.component.html',
})
export class HomePageComponent {
  private readonly api = inject(CatalogApi);
  private readonly modal = inject(NgbModal);
  readonly auth = inject(AuthService);

  readonly home = signal<Home | null>(null);
  /** Selected category tab (0 = All) and its paged courses. */
  readonly selectedDepartment = signal(0);
  readonly departmentDescription = signal<string | null>(null);
  readonly tabCourses = signal<PagedResult<CourseCard> | null>(null);

  /** Legacy hides the courses the current user is already enrolled in. */
  readonly topCourses = computed(() => this.notEnrolled(this.home()?.topCourses));
  readonly recommendations = computed(() => this.notEnrolled(this.home()?.recommendations));

  constructor() {
    this.api.home().subscribe((home) => {
      this.home.set(home);
      this.tabCourses.set(home.courses);
    });
  }

  selectDepartment(departmentId: number, page = 1): void {
    this.selectedDepartment.set(departmentId);
    this.api.coursesByDepartment(departmentId, { page }).subscribe((result) => {
      this.departmentDescription.set(result.departmentDescription);
      this.tabCourses.set(result.courses);
    });
  }

  openTeachModal(): void {
    this.modal.open(InstructorRegisterModalComponent, { size: 'xl', centered: true, scrollable: true });
  }

  scrollToCourses(): void {
    document.getElementById('courses')?.scrollIntoView({ behavior: 'smooth' });
  }

  private notEnrolled(courses?: CourseCard[]): CourseCard[] {
    const enrolled = this.home()?.enrolledCourseIds ?? [];
    return (courses ?? []).filter((c) => !enrolled.includes(c.id));
  }
}
