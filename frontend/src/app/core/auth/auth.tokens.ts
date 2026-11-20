import { HttpContextToken } from '@angular/common/http';

/** Set on a request to skip the "refresh token on 401" logic (used by the auth calls themselves). */
export const SKIP_AUTH_REFRESH = new HttpContextToken<boolean>(() => false);
