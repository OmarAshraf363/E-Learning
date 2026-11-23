import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../../../core/api/api-client';
import { Cart, CartSummary, CheckoutResult } from './cart.models';

@Injectable({ providedIn: 'root' })
export class CartApi {
  private readonly api = inject(ApiClient);

  get(): Observable<Cart> {
    return this.api.get<Cart>('cart');
  }

  summary(): Observable<CartSummary> {
    return this.api.get<CartSummary>('cart/summary');
  }

  add(courseId: number): Observable<Cart> {
    return this.api.post<Cart>('cart/items', { courseId });
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
