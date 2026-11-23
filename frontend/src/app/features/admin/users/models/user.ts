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
  departmentId: number | null;
}

/**
 * Password is required on create and ignored on edit.
 * departmentId links an instructor to a department (the instructor then creates courses in it only).
 */
export interface UserRequest {
  userName: string;
  email: string;
  role: string;
  fullName: string | null;
  password: string | null;
  departmentId: number | null;
}
