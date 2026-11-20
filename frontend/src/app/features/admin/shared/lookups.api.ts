import { inject, Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';
import { ApiClient } from '../../../core/api/api-client';

export interface LookupItem<TId = number> {
  id: TId;
  name: string;
}

/** Dropdown data for the admin forms (GET /api/admin/lookups/...). */
@Injectable({ providedIn: 'root' })
export class LookupsApi {
  private readonly api = inject(ApiClient);

  departments = (): Observable<LookupItem[]> => this.get('departments');
  tracks = (): Observable<LookupItem[]> => this.get('tracks');
  courses = (): Observable<LookupItem[]> => this.get('courses');
  rooms = (): Observable<LookupItem[]> => this.get('rooms');
  academicYears = (): Observable<LookupItem[]> => this.get('academic-years');
  instructors = (): Observable<LookupItem<string>[]> => this.get('instructors');
  users = (): Observable<LookupItem<string>[]> => this.get('users');

  private get<T>(name: string): Observable<T> {
    return this.api.get<T>(`admin/lookups/${name}`).pipe(shareReplay(1));
  }
}
