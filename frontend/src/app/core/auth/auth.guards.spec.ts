import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter, Router, UrlTree } from '@angular/router';
import { roleGuard } from './auth.guards';
import { AuthService } from './auth.service';

describe('roleGuard', () => {
  const run = () =>
    TestBed.runInInjectionContext(() => roleGuard('Admin')({} as never, { url: '/admin' } as never));

  beforeEach(() => {
    localStorage.clear();
    TestBed.configureTestingModule({ providers: [provideRouter([]), provideHttpClient()] });
  });

  it('redirects anonymous users to login with returnUrl', () => {
    const result = run() as UrlTree;
    expect(TestBed.inject(Router).serializeUrl(result)).toBe('/auth/login?returnUrl=%2Fadmin');
  });

  it('allows users that have the role', () => {
    TestBed.inject(AuthService).setSession({
      accessToken: 't',
      accessTokenExpiresAt: '',
      refreshToken: 'r',
      user: { id: '1', userName: 'a', email: 'a@a', fullName: 'A', roles: ['Admin'] },
    });
    expect(run()).toBe(true);
  });
});
