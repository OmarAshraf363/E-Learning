import { computed, signal } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { BaseApiService, EntityId } from '../api/base-api.service';
import { QueryParams } from '../api/http-params.util';

/**
 * Generic signal-based state for a list of entities backed by a BaseApiService.
 * Pages read the signals; they call load / save / remove.
 *
 * Usage:
 *   @Injectable({ providedIn: 'root' })
 *   export class DepartmentsStore extends CrudStore<Department, DepartmentRequest> {
 *     constructor() { super(inject(DepartmentsApi), (d) => d.id); }
 *   }
 *
 *   // in a page component
 *   readonly store = inject(DepartmentsStore);
 *   ngOnInit() { this.store.load(); }
 *   // template: @for (d of store.items(); track d.id) { ... }
 */
export abstract class CrudStore<T, TRequest = Partial<T>, TId extends EntityId = number> {
  readonly items = signal<T[]>([]);
  readonly loading = signal(false);
  readonly error = signal<unknown>(null);
  readonly isEmpty = computed(() => !this.loading() && this.items().length === 0);

  protected constructor(
    protected readonly api: BaseApiService<T, TRequest, TId>,
    private readonly idOf: (item: T) => TId,
  ) {}

  async load(query?: QueryParams): Promise<void> {
    await this.run(async () => this.items.set(await firstValueFrom(this.api.getAll(query))));
  }

  /** Creates when `id` is undefined, otherwise updates. Returns the saved item. */
  async save(body: TRequest, id?: TId): Promise<T> {
    return this.run(async () => {
      const saved = await firstValueFrom(id === undefined ? this.api.create(body) : this.api.update(id, body));
      this.items.update((list) =>
        id === undefined ? [...list, saved] : list.map((item) => (this.idOf(item) === id ? saved : item)),
      );
      return saved;
    });
  }

  async remove(id: TId): Promise<void> {
    await this.run(async () => {
      await firstValueFrom(this.api.delete(id));
      this.items.update((list) => list.filter((item) => this.idOf(item) !== id));
    });
  }

  /** Wraps an async call with loading / error handling. Errors are re-thrown to the caller. */
  protected async run<R>(action: () => Promise<R>): Promise<R> {
    this.loading.set(true);
    this.error.set(null);
    try {
      return await action();
    } catch (err) {
      this.error.set(err);
      throw err;
    } finally {
      this.loading.set(false);
    }
  }
}
