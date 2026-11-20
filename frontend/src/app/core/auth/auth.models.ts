export type Role = 'Admin' | 'Instructor' | 'Student';

export const Roles = {
  Admin: 'Admin',
  Instructor: 'Instructor',
  Student: 'Student',
} as const satisfies Record<Role, Role>;

export interface CurrentUser {
  id: string;
  userName: string;
  email: string;
  fullName: string;
  picture?: string | null;
  roles: Role[];
}

/** Returned by /auth/login, /auth/register and /auth/refresh. */
export interface AuthResponse {
  accessToken: string;
  accessTokenExpiresAt: string;
  refreshToken: string;
  user: CurrentUser;
}
