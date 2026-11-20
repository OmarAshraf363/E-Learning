import { HttpParams } from '@angular/common/http';

/** Any flat object of query-string values. `null` / `undefined` / '' are skipped. */
export type QueryParams = Record<string, string | number | boolean | null | undefined | (string | number)[]>;

export function toHttpParams(query?: QueryParams): HttpParams {
  let params = new HttpParams();
  if (!query) return params;

  for (const [key, value] of Object.entries(query)) {
    if (value === null || value === undefined || value === '') continue;
    if (Array.isArray(value)) value.forEach((v) => (params = params.append(key, String(v))));
    else params = params.set(key, String(value));
  }
  return params;
}

/**
 * Converts an object into multipart FormData that ASP.NET model binding understands:
 * - File / Blob      -> file part
 * - string[]         -> key=a&key=b
 * - object[]         -> key[0].prop=...
 * - Date             -> ISO string
 */
export function toFormData(body: object): FormData {
  const form = new FormData();

  const append = (key: string, value: unknown): void => {
    if (value === null || value === undefined) return;
    if (value instanceof Blob) form.append(key, value);
    else if (value instanceof Date) form.append(key, value.toISOString());
    else if (Array.isArray(value)) {
      value.forEach((item, i) =>
        typeof item === 'object' && !(item instanceof Blob) ? append(`${key}[${i}]`, item) : append(key, item),
      );
    } else if (typeof value === 'object') {
      Object.entries(value).forEach(([k, v]) => append(`${key}.${k}`, v));
    } else form.append(key, String(value));
  };

  Object.entries(body).forEach(([key, value]) => append(key, value));
  return form;
}
