import { HttpContext } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { finalize, Observable, shareReplay, tap } from 'rxjs';
import { ApiClient } from '../api/api-client';
import { SKIP_AUTH_REFRESH } from './auth.tokens';
import { AuthResponse, CurrentUser, Role, Roles } from './auth.models';
import { TokenStorage } from './token.storage';

/** Login state of the app (signals) + login / logout / refresh calls. */
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly api = inject(ApiClient);
  private readonly storage = inject(TokenStorage);
  private readonly router = inject(Router);

  private readonly session = signal<AuthResponse | null>(this.storage.read());
  private refreshInFlight$: Observable<AuthResponse> | null = null;

  readonly user = computed<CurrentUser | null>(() => this.session()?.user ?? null);
  readonly isLoggedIn = computed(() => this.user() !== null);

  get accessToken(): string | null {
    return this.session()?.accessToken ?? null;
  }

  hasRole(...roles: Role[]): boolean {
    const userRoles = this.user()?.roles ?? [];
    return roles.some((role) => userRoles.includes(role));
  }

  login(email: string, password: string): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('auth/login', { email, password }).pipe(tap((s) => this.setSession(s)));
  }

  /** Visitor registration: creates a Student account and signs in (legacy Register page). */
  register(body: { fullName: string; email: string; password: string; confirmPassword: string }): Observable<AuthResponse> {
    return this.api.post<AuthResponse>('auth/register', body).pipe(tap((s) => this.setSession(s)));
  }

  /** Updates the cached user after a profile change (e.g. new picture). */
  patchUser(changes: Partial<AuthResponse['user']>): void {
    const current = this.session();
    if (current) this.setSession({ ...current, user: { ...current.user, ...changes } });
  }

  /** Stores a session returned by any auth endpoint (login / register). */
  setSession(session: AuthResponse): void {
    this.storage.write(session);
    this.session.set(session);
  }

  /** Exchanges the refresh token for a new access token. Concurrent callers share one request. */
  refresh(): Observable<AuthResponse> {
    this.refreshInFlight$ ??= this.api
      .post<AuthResponse>(
        'auth/refresh',
        { refreshToken: this.session()?.refreshToken },
        { context: new HttpContext().set(SKIP_AUTH_REFRESH, true) },
      )
      .pipe(
        tap((s) => this.setSession(s)),
        finalize(() => (this.refreshInFlight$ = null)),
        shareReplay(1),
      );
    return this.refreshInFlight$;
  }

  logout(redirect = true): void {
    const refreshToken = this.session()?.refreshToken;
    if (refreshToken) {
      this.api
        .post('auth/logout', { refreshToken }, { context: new HttpContext().set(SKIP_AUTH_REFRESH, true) })
        .subscribe({ error: () => undefined });
    }
    this.storage.clear();
    this.session.set(null);
    if (redirect) void this.router.navigateByUrl('/auth/login');
  }

  /**
   * Landing page per role - same as the MVC Home/Index redirects:
   * Admin -> admin dashboard, Instructor -> instructor dashboard, Student / visitor -> public home.
   */
  homeUrl(): string {
    if (this.hasRole(Roles.Admin)) return '/admin';
    if (this.hasRole(Roles.Instructor)) return '/instructor';
    return '/';
  }
}
