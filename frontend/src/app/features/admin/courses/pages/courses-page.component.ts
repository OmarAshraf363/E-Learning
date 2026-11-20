import { Component, inject } from '@angular/core';
import { NotifyService } from '../../../../core/ui/notify.service';
import { CoursesTableComponent } from '../courses-table.component';
import { CoursesStore } from '../courses.store';

/** Legacy Admin/Course/Index. */
@Component({
  selector: 'app-courses-page',
  imports: [CoursesTableComponent],
  template: `<app-courses-table [courses]="store.items()" (remove)="remove($event)" />`,
})
export class CoursesPageComponent {
  readonly store = inject(CoursesStore);
  private readonly notify = inject(NotifyService);

  constructor() {
    void this.store.load();
  }

  async remove(id: number): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await this.store.remove(id);
    this.notify.success('Deleted successfully');
  }
}
