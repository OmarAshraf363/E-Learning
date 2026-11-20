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
export abstract class CrudStore<T, TRequest = Partial<T>, TId extends EntityId = number, TDetails = T> {
  readonly items = signal<T[]>([]);
  readonly loading = signal(false);
  readonly error = signal<unknown>(null);
  readonly isEmpty = computed(() => !this.loading() && this.items().length === 0);

  /** Query used by the last load(), re-used to refresh the list after a save. */
  private lastQuery?: QueryParams;

  protected constructor(
    protected readonly api: BaseApiService<T, TRequest, TId, TDetails>,
    private readonly idOf: (item: T) => TId,
  ) {}

  async load(query?: QueryParams): Promise<void> {
    this.lastQuery = query;
    await this.run(async () => this.items.set(await firstValueFrom(this.api.getAll(query))));
  }

  /** Creates when `id` is undefined, otherwise updates, then reloads the list. Returns the saved item. */
  async save(body: TRequest, id?: TId): Promise<TDetails> {
    const saved = await this.run(() =>
      firstValueFrom(id === undefined ? this.api.create(body) : this.api.update(id, body)),
    );
    await this.load(this.lastQuery);
    return saved;
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
