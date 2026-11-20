/** Row of the "Manage Students / Instructors" table. */
export interface UserListItem {
  id: string;
  userName: string;
  email: string;
  fullName: string;
  userType: string;
  isLockedOut: boolean;
}

export interface UserDetails {
  id: string;
  userName: string;
  email: string;
  fullName: string;
  role: string;
}

/** Password is required on create and ignored on edit. */
export interface UserRequest {
  userName: string;
  email: string;
  role: string;
  fullName: string | null;
  password: string | null;
}
