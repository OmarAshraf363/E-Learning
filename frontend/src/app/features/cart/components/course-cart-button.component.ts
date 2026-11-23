import { Component, computed, inject, input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CartStore } from '../data-access/cart.store';

/**
 * The buy button of a course (course cards and the course details page).
 * Business rule (second plan): no buying a course twice:
 *  - enrolled        -> "Open course" (the student's course player)
 *  - already in cart -> "Go to Cart"
 *  - otherwise       -> the given label; goes to /cart?add=<id> (legacy Cart/AddToCart)
 */
@Component({
  selector: 'app-course-cart-button',
  imports: [RouterLink],
  template: `
    @switch (state()) {
      @case ('enrolled') {
        <a [routerLink]="['/student/courses', courseId()]" [class]="cssClass()" [style]="buttonStyle()">Open course</a>
      }
      @case ('in-cart') {
        <a routerLink="/cart" [class]="cssClass()" [style]="buttonStyle()">Go to Cart</a>
      }
      @default {
        <a routerLink="/cart" [queryParams]="{ add: courseId() }" [class]="cssClass()" [style]="buttonStyle()">{{ label() }}</a>
      }
    }
  `,
})
export class CourseCartButtonComponent {
  private readonly cart = inject(CartStore);

  readonly courseId = input.required<number>();
  readonly label = input('Add to Cart');
  readonly cssClass = input('btn cus-btn');
  readonly buttonStyle = input('');

  readonly state = computed(() =>
    this.cart.isEnrolled(this.courseId()) ? 'enrolled' : this.cart.isInCart(this.courseId()) ? 'in-cart' : 'add',
  );
}
