import { effect, inject, Injectable, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { AuthService } from '../../../core/auth/auth.service';
import { CartApi } from './cart.api';
import { Cart } from './cart.models';

/**
 * Cart state shared by the cart page and the navbar badge (legacy CartIcon view component).
 * The badge count is reloaded whenever the logged-in user changes.
 */
@Injectable({ providedIn: 'root' })
export class CartStore {
  private readonly api = inject(CartApi);
  private readonly auth = inject(AuthService);

  readonly cart = signal<Cart | null>(null);
  readonly count = signal(0);
  readonly loading = signal(false);

  constructor() {
    effect(() => {
      if (this.auth.user()) this.refreshCount();
      else {
        this.count.set(0);
        this.cart.set(null);
      }
    });
  }

  refreshCount(): void {
    this.api.count().subscribe({ next: (c) => this.count.set(c), error: () => this.count.set(0) });
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
  }
}
