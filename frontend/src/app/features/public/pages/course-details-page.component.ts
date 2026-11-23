import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { StarRatingComponent } from '../../../shared/components/star-rating.component';
import { CourseGridComponent } from '../components/course-grid.component';
import { CourseCartButtonComponent } from '../../cart/components/course-cart-button.component';
import { CatalogApi } from '../data-access/catalog.api';
import { CourseDetails } from '../data-access/catalog.models';

/**
 * Legacy Customer/Home/Details: demo video, info, objectives / topics tabs, instructor,
 * related courses, more by the instructor, reviews with the star filter (?starFilter=).
 * (The legacy sort dropdown / "helpful" / "load more" called functions that did not exist.)
 */
@Component({
  selector: 'app-course-details-page',
  imports: [RouterLink, CurrencyPipe, DatePipe, FileUrlPipe, StarRatingComponent, CourseGridComponent, CourseCartButtonComponent],
  templateUrl: './course-details-page.component.html',
  styleUrl: './course-details-page.component.scss',
})
export class CourseDetailsPageComponent {
  private readonly api = inject(CatalogApi);
  private readonly router = inject(Router);

  readonly id = input.required<string>();
  readonly starFilter = input<string>();

  readonly course = signal<CourseDetails | null>(null);
  readonly tab = signal<'objectives' | 'topics'>('objectives');
  readonly stars = [5, 4, 3, 2, 1];

  readonly breakdown = computed(() => {
    const summary = this.course()?.reviewSummary;
    if (!summary) return [];
    const total = summary.totalReviews || 1; // legacy: prevent divide-by-zero
    return this.stars.map((s) => {
      const count = summary.countByStars[s] ?? 0;
      return { stars: s, percentage: Math.floor((count / total) * 100) };
    });
  });

  constructor() {
    effect(() => {
      const star = this.starFilter() ? +this.starFilter()! : null;
      this.api.courseDetails(+this.id(), star).subscribe((c) => this.course.set(c));
    });
  }

  filterReviews(star: number | null): void {
    void this.router.navigate([], { queryParams: { starFilter: star }, replaceUrl: true });
  }

  selectedStar(): number | null {
    return this.starFilter() ? +this.starFilter()! : null;
  }
}
