import { Component, computed, input, output } from '@angular/core';

/**
 * Bootstrap pager (same markup as the legacy _partialPagination).
 *   <app-pagination [page]="result.page" [totalPages]="result.totalPages" (pageChange)="load($event)" />
 */
@Component({
  selector: 'app-pagination',
  template: `
    @if (totalPages() > 1) {
      <nav aria-label="Page navigation">
        <ul class="pagination justify-content-center">
          <li class="page-item" [class.disabled]="page() <= 1">
            <button type="button" class="page-link" aria-label="Previous" (click)="go(page() - 1)">&laquo;</button>
          </li>
          @for (p of pages(); track p) {
            <li class="page-item" [class.active]="p === page()">
              <button type="button" class="page-link" (click)="go(p)">{{ p }}</button>
            </li>
          }
          <li class="page-item" [class.disabled]="page() >= totalPages()">
            <button type="button" class="page-link" aria-label="Next" (click)="go(page() + 1)">&raquo;</button>
          </li>
        </ul>
      </nav>
    }
  `,
})
export class PaginationComponent {
  readonly page = input.required<number>();
  readonly totalPages = input.required<number>();
  readonly pageChange = output<number>();

  readonly pages = computed(() => Array.from({ length: this.totalPages() }, (_, i) => i + 1));

  go(target: number): void {
    if (target >= 1 && target <= this.totalPages() && target !== this.page()) this.pageChange.emit(target);
  }
}
