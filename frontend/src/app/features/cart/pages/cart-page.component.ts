import { CurrencyPipe } from '@angular/common';
import { Component, computed, inject, input, OnInit, signal } from '@angular/core';
import { Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { ProblemDetails } from '../../../core/models/problem-details';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { CourseGridComponent } from '../../public/components/course-grid.component';
import { CartApi } from '../data-access/cart.api';
import { CartStore } from '../data-access/cart.store';

/**
 * Legacy Customer/Cart/Index. "?add=<courseId>" adds the course first
 * (legacy Cart/AddToCart redirected to this page), then the param is removed from the URL.
 */
@Component({
  selector: 'app-cart-page',
  imports: [CurrencyPipe, FileUrlPipe, CourseGridComponent],
  templateUrl: './cart-page.component.html',
})
export class CartPageComponent implements OnInit {
  readonly store = inject(CartStore);
  private readonly api = inject(CartApi);
  private readonly router = inject(Router);

  readonly add = input<string>();
  readonly error = signal<string | null>(null);
  readonly redirecting = signal(false);

  /** Same summary as the legacy page (tax shown as 10% of the subtotal). */
  readonly subtotal = computed(() => this.store.cart()?.subtotal ?? 0);
  readonly tax = computed(() => this.subtotal() * 0.1);
  readonly total = computed(() => this.subtotal() * 1.1);

  async ngOnInit(): Promise<void> {
    const add = this.add();
    if (add) {
      await this.store.add(+add);
      void this.router.navigate([], { queryParams: { add: null }, replaceUrl: true });
    } else {
      await this.store.load();
    }
  }

  remove(itemId: number): void {
    void this.store.remove(itemId);
  }

  async checkout(): Promise<void> {
    this.error.set(null);
    this.redirecting.set(true);
    try {
      const { url } = await firstValueFrom(this.api.checkout());
      window.location.href = url; // Stripe Checkout page
    } catch (err) {
      this.redirecting.set(false);
      if (err instanceof HttpErrorResponse) this.error.set((err.error as ProblemDetails)?.detail ?? 'Failed to start payment session.');
    }
  }
}
