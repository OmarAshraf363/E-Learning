import { Injectable } from '@angular/core';
import { AuthResponse } from './auth.models';

const KEY = 'bfcai.auth';

/** Persists the auth session in localStorage. */
@Injectable({ providedIn: 'root' })
export class TokenStorage {
  read(): AuthResponse | null {
    try {
      const raw = localStorage.getItem(KEY);
      return raw ? (JSON.parse(raw) as AuthResponse) : null;
    } catch {
      return null;
    }
  }

  write(session: AuthResponse): void {
    try {
      localStorage.setItem(KEY, JSON.stringify(session));
    } catch {
      /* storage unavailable (private mode) - session lives in memory only */
    }
  }

  clear(): void {
    try {
      localStorage.removeItem(KEY);
    } catch {
      /* ignore */
    }
  }
}
