import { effect, inject, Injectable, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { AuthService } from '../../../core/auth/auth.service';
import { CartApi } from './cart.api';
import { Cart } from './cart.models';

/**
 * Cart state shared by the cart page, the navbar badge (legacy CartIcon view component) and the
 * course buttons. The summary (count, courses in the cart, enrolled courses) is reloaded whenever
 * the logged-in user changes.
 * Business rule (second plan): a course is bought once, so the buttons show "Open course" for an
 * enrolled course and "Go to Cart" for a course already in the cart.
 */
@Injectable({ providedIn: 'root' })
export class CartStore {
  private readonly api = inject(CartApi);
  private readonly auth = inject(AuthService);

  readonly cart = signal<Cart | null>(null);
  readonly count = signal(0);
  readonly cartCourseIds = signal<number[]>([]);
  readonly enrolledCourseIds = signal<number[]>([]);
  readonly loading = signal(false);

  constructor() {
    effect(() => {
      if (this.auth.user()) this.refreshSummary();
      else this.clear();
    });
  }

  isInCart(courseId: number): boolean {
    return this.cartCourseIds().includes(courseId);
  }

  isEnrolled(courseId: number): boolean {
    return this.enrolledCourseIds().includes(courseId);
  }

  refreshSummary(): void {
    this.api.summary().subscribe({
      next: (s) => {
        this.count.set(s.count);
        this.cartCourseIds.set(s.cartCourseIds);
        this.enrolledCourseIds.set(s.enrolledCourseIds);
      },
      error: () => this.clear(),
    });
  }

  async load(): Promise<void> {
    this.loading.set(true);
    try {
      this.set(await firstValueFrom(this.api.get()));
    } finally {
      this.loading.set(false);
    }
  }

  async add(courseId: number): Promise<void> {
    this.set(await firstValueFrom(this.api.add(courseId)));
  }

  async remove(itemId: number): Promise<void> {
    this.set(await firstValueFrom(this.api.remove(itemId)));
  }

  private set(cart: Cart): void {
    this.cart.set(cart);
    this.count.set(cart.items.length);
    this.cartCourseIds.set(cart.items.map((i) => i.courseId));
  }

  private clear(): void {
    this.count.set(0);
    this.cart.set(null);
    this.cartCourseIds.set([]);
    this.enrolledCourseIds.set([]);
  }
}
