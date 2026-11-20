import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { AuthService } from '../../core/auth/auth.service';
import { UserMenuComponent } from '../shared/user-menu.component';

/** Public storefront layout (legacy _RegistreationLayout). */
@Component({
  selector: 'app-public-layout',
  imports: [RouterOutlet, RouterLink, FormsModule, UserMenuComponent],
  templateUrl: './public-layout.component.html',
})
export class PublicLayoutComponent {
  readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  readonly mobileMenuOpen = signal(false);
  search = '';

  onSearch(): void {
    void this.router.navigate(['/courses'], { queryParams: { search: this.search || null } });
  }
}
