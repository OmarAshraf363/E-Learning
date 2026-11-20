import { Component, inject, signal } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';
import { UserMenuComponent } from '../shared/user-menu.component';
import { CatalogApi } from '../../features/public/data-access/catalog.api';
import { CartStore } from '../../features/cart/data-access/cart.store';

/** Public storefront layout (legacy _RegistreationLayout). */
@Component({
  selector: 'app-public-layout',
  imports: [RouterOutlet, RouterLink, FormsModule, UserMenuComponent],
  templateUrl: './public-layout.component.html',
})
export class PublicLayoutComponent {
  readonly auth = inject(AuthService);
  readonly cart = inject(CartStore);
  private readonly router = inject(Router);

  readonly mobileMenuOpen = signal(false);
  /** Legacy Categories view component: departments with their courses. */
  readonly navCategories = toSignal(inject(CatalogApi).navCategories(), { initialValue: [] });
  search = '';

  onSearch(): void {
    void this.router.navigate(['/courses'], { queryParams: { search: this.search || null } });
  }
}
