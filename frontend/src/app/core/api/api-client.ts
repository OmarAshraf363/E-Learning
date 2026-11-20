import { HttpClient, HttpContext } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { QueryParams, toFormData, toHttpParams } from './http-params.util';

export interface RequestOptions {
  /** Extra flags for interceptors, e.g. new HttpContext().set(SKIP_ERROR_ALERT, true) */
  context?: HttpContext;
}

/**
 * Thin wrapper over HttpClient: prefixes the API base URL and builds query params / FormData.
 * Every API call in the app goes through this class.
 */
@Injectable({ providedIn: 'root' })
export class ApiClient {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = environment.apiUrl;

  get<T>(path: string, query?: QueryParams, options?: RequestOptions): Observable<T> {
    return this.http.get<T>(this.url(path), { params: toHttpParams(query), context: options?.context });
  }

  post<T>(path: string, body: unknown = {}, options?: RequestOptions): Observable<T> {
    return this.http.post<T>(this.url(path), body, { context: options?.context });
  }

  put<T>(path: string, body: unknown = {}, options?: RequestOptions): Observable<T> {
    return this.http.put<T>(this.url(path), body, { context: options?.context });
  }

  delete<T = void>(path: string, options?: RequestOptions): Observable<T> {
    return this.http.delete<T>(this.url(path), { context: options?.context });
  }

  /** multipart/form-data request (forms with files). */
  postForm<T>(path: string, body: object, options?: RequestOptions): Observable<T> {
    return this.http.post<T>(this.url(path), toFormData(body), { context: options?.context });
  }

  putForm<T>(path: string, body: object, options?: RequestOptions): Observable<T> {
    return this.http.put<T>(this.url(path), toFormData(body), { context: options?.context });
  }

  /** Downloads a file as Blob. */
  download(path: string, query?: QueryParams): Observable<Blob> {
    return this.http.get(this.url(path), { params: toHttpParams(query), responseType: 'blob' });
  }

  private url(path: string): string {
    return `${this.baseUrl}/${path.replace(/^\/+/, '')}`;
  }
}
