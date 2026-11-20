import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

/** Legacy checkout/cancel. */
@Component({
  selector: 'app-checkout-cancel-page',
  imports: [RouterLink],
  template: `
    <section class="container py-5 text-center">
      <div class="row">
        <div class="col-md-8 offset-md-2">
          <div class="card shadow-sm p-4">
            <div class="card-body">
              <h1 class="display-4 text-danger mb-4"><i class="fas fa-times-circle"></i> Payment Canceled</h1>
              <p class="lead">It seems your payment was not completed. Please try again or contact support if you have any issues.</p>
              <a routerLink="/cart" class="btn btn-primary mt-4 me-2">Return to Cart</a>
              <a routerLink="/" class="btn btn-secondary mt-4">Continue Browsing</a>
            </div>
          </div>
        </div>
      </div>
    </section>
  `,
  styles: `.lead { font-size: 1.25rem; }`,
})
export class CheckoutCancelPageComponent {}
