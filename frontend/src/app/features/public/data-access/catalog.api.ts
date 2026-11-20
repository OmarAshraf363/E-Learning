import { inject, Injectable } from '@angular/core';
import { Observable, shareReplay } from 'rxjs';
import { ApiClient } from '../../../core/api/api-client';
import { PagedResult, PaginationParams } from '../../../core/models/paged-result';
import {
  Category, CourseCard, CourseDetails, CourseFilter, DepartmentCard, DepartmentCourses, Home, NavCategory, PublicTrack,
} from './catalog.models';

/** Public storefront endpoints (GET /api/catalog/...). */
@Injectable({ providedIn: 'root' })
export class CatalogApi {
  private readonly api = inject(ApiClient);
  private nav$?: Observable<NavCategory[]>;

  home(pagination: PaginationParams = {}): Observable<Home> {
    return this.api.get<Home>('catalog/home', { ...pagination });
  }

  coursesByDepartment(departmentId: number, pagination: PaginationParams = {}): Observable<DepartmentCourses> {
    return this.api.get<DepartmentCourses>('catalog/departments/courses', { departmentId, ...pagination });
  }

  courses(filter: CourseFilter, pagination: PaginationParams): Observable<PagedResult<CourseCard>> {
    return this.api.get<PagedResult<CourseCard>>('catalog/courses', { ...filter, ...pagination });
  }

  courseDetails(id: number, starFilter?: number | null): Observable<CourseDetails> {
    return this.api.get<CourseDetails>(`catalog/courses/${id}`, { starFilter });
  }

  categories(): Observable<DepartmentCard[]> {
    return this.api.get<DepartmentCard[]>('catalog/categories');
  }

  category(id: number): Observable<Category> {
    return this.api.get<Category>(`catalog/categories/${id}`);
  }

  topic(word: string): Observable<CourseCard[]> {
    return this.api.get<CourseCard[]>(`catalog/topics/${encodeURIComponent(word)}`);
  }

  track(id: number): Observable<PublicTrack> {
    return this.api.get<PublicTrack>(`catalog/tracks/${id}`);
  }

  /** Navbar categories menu (loaded once per session). */
  navCategories(): Observable<NavCategory[]> {
    return (this.nav$ ??= this.api.get<NavCategory[]>('catalog/nav').pipe(shareReplay(1)));
  }
}
