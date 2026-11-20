import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { PagedResult } from '../models/paged-result';
import { ApiClient } from './api-client';
import { QueryParams } from './http-params.util';

export type EntityId = string | number;

/**
 * Generic CRUD service for one REST resource.
 *
 *   GET    /{resource}            -> getAll(query)
 *   GET    /{resource}/paged      -> getPaged(query)
 *   GET    /{resource}/{id}       -> getById(id)
 *   POST   /{resource}            -> create(body)
 *   PUT    /{resource}/{id}       -> update(id, body)
 *   DELETE /{resource}/{id}       -> delete(id)
 *
 * Usage:
 *   @Injectable({ providedIn: 'root' })
 *   export class DepartmentsApi extends BaseApiService<Department, DepartmentRequest> {
 *     protected readonly resource = 'admin/departments';
 *   }
 *
 * Set `useFormData = true` when the create/update bodies contain files.
 * Add extra endpoint methods in the subclass using `this.api`.
 */
export abstract class BaseApiService<T, TRequest = Partial<T>, TId extends EntityId = number> {
  protected readonly api = inject(ApiClient);
  protected abstract readonly resource: string;
  protected readonly useFormData: boolean = false;

  getAll(query?: QueryParams): Observable<T[]> {
    return this.api.get<T[]>(this.resource, query);
  }

  getPaged(query?: QueryParams): Observable<PagedResult<T>> {
    return this.api.get<PagedResult<T>>(`${this.resource}/paged`, query);
  }

  getById(id: TId): Observable<T> {
    return this.api.get<T>(`${this.resource}/${id}`);
  }

  create(body: TRequest): Observable<T> {
    return this.useFormData
      ? this.api.postForm<T>(this.resource, body as object)
      : this.api.post<T>(this.resource, body);
  }

  update(id: TId, body: TRequest): Observable<T> {
    return this.useFormData
      ? this.api.putForm<T>(`${this.resource}/${id}`, body as object)
      : this.api.put<T>(`${this.resource}/${id}`, body);
  }

  delete(id: TId): Observable<void> {
    return this.api.delete(`${this.resource}/${id}`);
  }
}
