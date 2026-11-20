/** RFC 7807 error body returned by the API for every failed request. */
export interface ProblemDetails {
  status?: number;
  title?: string;
  detail?: string;
  /** Field validation errors (400): { fieldName: ['message', ...] } */
  errors?: Record<string, string[]>;
}
