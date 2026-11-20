import { CurrencyPipe, DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, inject, input, OnInit, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ProblemDetails } from '../../../core/models/problem-details';
import { CartApi } from '../data-access/cart.api';
import { CartStore } from '../data-access/cart.store';
import { CheckoutResult } from '../data-access/cart.models';

/**
 * Legacy checkout/success. Stripe redirects here with ?session_id=...; the API verifies the
 * payment with Stripe before enrolling (the legacy page enrolled without any check).
 */
@Component({
  selector: 'app-checkout-success-page',
  imports: [RouterLink, CurrencyPipe, DatePipe],
  template: `
    <section class="container py-5 text-center">
      <div class="row">
        <div class="col-md-8 offset-md-2">
          <div class="card shadow-sm p-4">
            <div class="card-body">
              @if (result(); as r) {
                <h1 class="display-4 text-success mb-4"><i class="fas fa-check-circle"></i> Payment Successful!</h1>
                <p class="lead">Thank you for your purchase. Your order has been placed successfully, and your payment is being processed.</p>
                <div class="order-summary mt-4">
                  <h5 class="mb-3">Order Summary</h5>
                  <p><strong>Order Date:</strong> {{ r.orderDate | date: 'MMMM dd, yyyy' }}</p>
                  <p><strong>Order Total:</strong> {{ r.total | currency }}</p>
                  <p><strong>Status:</strong> Your order is being processed and will be available shortly.</p>
                </div>
                <a routerLink="/" class="btn btn-primary mt-4">Continue Shopping</a>
              } @else if (error()) {
                <h1 class="display-5 text-danger mb-4"><i class="fas fa-times-circle"></i> Payment not confirmed</h1>
                <p class="lead">{{ error() }}</p>
                <a routerLink="/cart" class="btn btn-primary mt-4">Return to Cart</a>
              } @else {
                <div class="spinner-border text-primary" role="status"><span class="visually-hidden">Confirming payment...</span></div>
                <p class="mt-3">Confirming your payment...</p>
              }
            </div>
          </div>
        </div>
      </div>
    </section>
  `,
  styles: `
    .lead { font-size: 1.25rem; }
    .order-summary { background-color: #f8f9fa; padding: 20px; border-radius: 10px; border: 1px solid #dee2e6; }
  `,
})
export class CheckoutSuccessPageComponent implements OnInit {
  private readonly api = inject(CartApi);
  private readonly store = inject(CartStore);

  /** ?session_id= from Stripe */
  readonly session_id = input<string>();
  readonly result = signal<CheckoutResult | null>(null);
  readonly error = signal<string | null>(null);

  ngOnInit(): void {
    const sessionId = this.session_id();
    if (!sessionId) {
      this.error.set('Missing payment session.');
      return;
    }
    this.api.confirm(sessionId).subscribe({
      next: (r) => {
        this.result.set(r);
        this.store.refreshCount();
      },
      error: (err: unknown) =>
        this.error.set(err instanceof HttpErrorResponse ? ((err.error as ProblemDetails)?.detail ?? 'Payment could not be confirmed.') : 'Payment could not be confirmed.'),
    });
  }
}
