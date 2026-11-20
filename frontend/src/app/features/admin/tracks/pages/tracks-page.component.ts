import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { NotifyService } from '../../../../core/ui/notify.service';
import { TrackFormModalComponent } from '../components/track-form-modal.component';
import { TracksStore } from '../tracks.store';

/**
 * Legacy Admin/Track/Index (track cards).
 * The legacy page had no "New Track" button (create was only reachable by URL); one is added here.
 */
@Component({
  selector: 'app-tracks-page',
  imports: [RouterLink],
  templateUrl: './tracks-page.component.html',
  styleUrl: './tracks-page.component.scss',
})
export class TracksPageComponent {
  readonly store = inject(TracksStore);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);

  constructor() {
    void this.store.load();
  }

  openForm(id?: number): void {
    const ref = this.modal.open(TrackFormModalComponent, { centered: true });
    (ref.componentInstance as TrackFormModalComponent).id = id;
  }

  async remove(id: number): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await this.store.remove(id);
    this.notify.success('Deleted successfully');
  }
}
