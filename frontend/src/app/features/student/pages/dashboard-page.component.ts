import { CurrencyPipe, DatePipe } from '@angular/common';
import { Component, inject } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { StarRatingComponent } from '../../../shared/components/star-rating.component';
import { QuickAccessModalComponent } from '../components/quick-access-modal.component';
import { StudentApi } from '../data-access/student.api';

/** Legacy Student/DashBoard/Index. */
@Component({
  selector: 'app-student-dashboard-page',
  imports: [RouterLink, DatePipe, CurrencyPipe, FileUrlPipe, StarRatingComponent],
  templateUrl: './dashboard-page.component.html',
  styleUrl: './dashboard-page.component.scss',
})
export class DashboardPageComponent {
  private readonly modal = inject(NgbModal);
  readonly data = toSignal(inject(StudentApi).dashboard());

  /** Legacy shows "Comming" when the course has not started yet (its constant had the wrong value). */
  status(startDate: string | null): { text: string; css: string } {
    return startDate && new Date(startDate) > new Date()
      ? { text: 'Comming', css: 'text-warning' }
      : { text: 'In-Process', css: 'text-success' };
  }

  quickAccess(courseId: number): void {
    const ref = this.modal.open(QuickAccessModalComponent, { centered: true, size: 'lg', scrollable: true });
    (ref.componentInstance as QuickAccessModalComponent).courseId = courseId;
  }
}
