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
 *
 * Generic parameters:
 *   T        - item returned by the list endpoints
 *   TRequest - body sent to create / update
 *   TId      - id type (number, or string for users)
 *   TDetails - item returned by getById / create / update (defaults to T)
 */
export abstract class BaseApiService<T, TRequest = Partial<T>, TId extends EntityId = number, TDetails = T> {
  protected readonly api = inject(ApiClient);
  protected abstract readonly resource: string;
  protected readonly useFormData: boolean = false;

  getAll(query?: QueryParams): Observable<T[]> {
    return this.api.get<T[]>(this.resource, query);
  }

  getPaged(query?: QueryParams): Observable<PagedResult<T>> {
    return this.api.get<PagedResult<T>>(`${this.resource}/paged`, query);
  }

  getById(id: TId): Observable<TDetails> {
    return this.api.get<TDetails>(`${this.resource}/${id}`);
  }

  create(body: TRequest): Observable<TDetails> {
    return this.useFormData
      ? this.api.postForm<TDetails>(this.resource, body as object)
      : this.api.post<TDetails>(this.resource, body);
  }

  update(id: TId, body: TRequest): Observable<TDetails> {
    return this.useFormData
      ? this.api.putForm<TDetails>(`${this.resource}/${id}`, body as object)
      : this.api.put<TDetails>(`${this.resource}/${id}`, body);
  }

  delete(id: TId): Observable<void> {
    return this.api.delete(`${this.resource}/${id}`);
  }
}
