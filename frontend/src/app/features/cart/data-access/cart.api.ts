import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../../../core/api/api-client';
import { Cart, CheckoutResult } from './cart.models';

@Injectable({ providedIn: 'root' })
export class CartApi {
  private readonly api = inject(ApiClient);

  get(): Observable<Cart> {
    return this.api.get<Cart>('cart');
  }

  count(): Observable<number> {
    return this.api.get<number>('cart/count');
  }

  add(courseId: number, quantity = 1): Observable<Cart> {
    return this.api.post<Cart>('cart/items', { courseId, quantity });
  }

  remove(itemId: number): Observable<Cart> {
    return this.api.delete<Cart>(`cart/items/${itemId}`);
  }

  /** Returns the Stripe Checkout URL. */
  checkout(): Observable<{ url: string }> {
    return this.api.post<{ url: string }>('cart/checkout');
  }

  confirm(sessionId: string): Observable<CheckoutResult> {
    return this.api.post<CheckoutResult>(`cart/checkout/confirm?sessionId=${encodeURIComponent(sessionId)}`);
  }
}
