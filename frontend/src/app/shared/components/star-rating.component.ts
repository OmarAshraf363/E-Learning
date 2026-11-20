import { Component, computed, input } from '@angular/core';

/** Read-only 5-star rating (legacy: filled "fa-star" + empty stars). */
@Component({
  selector: 'app-star-rating',
  template: `
    @for (filled of stars(); track $index) {
      <span [class]="filled ? 'fa-solid fa-star checked' : 'fa-regular fa-star'"></span>
    }
  `,
  styles: `
    :host { display: inline-block; }
    .checked { color: #ffc107; }
    .fa-regular { color: #ffc107; }
  `,
})
export class StarRatingComponent {
  readonly rate = input<number | null>(0);
  readonly stars = computed(() => Array.from({ length: 5 }, (_, i) => i < (this.rate() ?? 0)));
}
