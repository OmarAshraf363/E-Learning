import { Component, computed, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from '../../../../core/ui/notify.service';
import { DepartmentFormModalComponent } from '../components/department-form-modal.component';
import { DepartmentsStore } from '../departments.store';

/** Legacy Admin/Department/Index. */
@Component({
  selector: 'app-departments-page',
  imports: [RouterLink],
  templateUrl: './departments-page.component.html',
  styleUrl: '../../shared/admin-table.scss',
})
export class DepartmentsPageComponent {
  readonly store = inject(DepartmentsStore);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);

  readonly search = signal('');
  readonly rows = computed(() => {
    const term = this.search().toLowerCase();
    return this.store.items().filter((d) => d.name.toLowerCase().includes(term));
  });

  constructor() {
    void this.store.load();
  }

  openForm(id?: number): void {
    const ref = this.modal.open(DepartmentFormModalComponent, { centered: true });
    (ref.componentInstance as DepartmentFormModalComponent).id = id;
  }

  async remove(id: number): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await this.store.remove(id);
    this.notify.success('Deleted successfully');
  }
}
