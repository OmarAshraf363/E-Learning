import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { PagedResult } from '../../../core/models/paged-result';
import { PaginationComponent } from '../../../shared/components/pagination.component';
import { CourseGridComponent } from '../components/course-grid.component';
import { CatalogApi } from '../data-access/catalog.api';
import { CourseCard } from '../data-access/catalog.models';

/**
 * Legacy Customer/Home/Courses + _partialFilterView.
 * All filters live in the URL (?search=&categoryId=&rate=&price=&page=), like the legacy GET form.
 */
@Component({
  selector: 'app-courses-page',
  imports: [FormsModule, CourseGridComponent, PaginationComponent],
  templateUrl: './courses-page.component.html',
  styleUrl: './courses-page.component.scss',
})
export class CoursesPageComponent {
  private readonly api = inject(CatalogApi);
  private readonly router = inject(Router);

  // Query params (bound by the router)
  readonly search = input<string>();
  readonly categoryId = input<string>();
  readonly rate = input<string>();
  readonly price = input<string>();
  readonly instructorId = input<string>();
  readonly page = input<string>();

  readonly categories = toSignal(this.api.categories(), { initialValue: [] });
  readonly result = signal<PagedResult<CourseCard> | null>(null);
  readonly ratings = [5, 4, 3, 2, 1];

  /** Filter form values (applied on "Apply Filters"). */
  readonly form = { categoryId: null as number | null, rate: null as number | null, price: 0 };
  readonly priceLabel = signal(0);
  readonly heading = computed(() => this.search());

  constructor() {
    effect(() => {
      this.form.categoryId = this.categoryId() ? +this.categoryId()! : null;
      this.form.rate = this.rate() ? +this.rate()! : null;
      this.form.price = this.price() ? +this.price()! : 0;
      this.priceLabel.set(this.form.price);

      this.api
        .courses(
          { search: this.search(), categoryId: this.form.categoryId, rate: this.form.rate, price: this.form.price || null, instructorId: this.instructorId() },
          { page: this.page() ? +this.page()! : 1, pageSize: 6 },
        )
        .subscribe((r) => this.result.set(r));
    });
  }

  apply(): void {
    void this.router.navigate(['/courses'], {
      queryParams: { search: this.search() || null, categoryId: this.form.categoryId, rate: this.form.rate, price: this.form.price || null },
    });
  }

  clear(): void {
    void this.router.navigate(['/courses']);
  }

  goToPage(page: number): void {
    void this.router.navigate([], { queryParams: { page }, queryParamsHandling: 'merge' });
  }
}
