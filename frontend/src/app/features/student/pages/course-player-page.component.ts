import { DatePipe } from '@angular/common';
import { Component, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { StarRatingComponent } from '../../../shared/components/star-rating.component';
import { FeedbackModalComponent } from '../components/feedback-modal.component';
import { SectionContentComponent } from '../components/section-content.component';
import { StudentApi } from '../data-access/student.api';
import { CourseFeedback, CoursePlayer } from '../data-access/student.models';

/** Legacy Student/DashBoard/CourseProcess: materials on the left, selected section, course feedback. */
@Component({
  selector: 'app-course-player-page',
  imports: [RouterLink, DatePipe, FileUrlPipe, StarRatingComponent, SectionContentComponent],
  templateUrl: './course-player-page.component.html',
  styleUrl: './course-player-page.component.scss',
})
export class CoursePlayerPageComponent {
  private readonly api = inject(StudentApi);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);

  readonly id = input.required<string>();
  readonly player = signal<CoursePlayer | null>(null);
  readonly selectedSection = signal<number | null>(null);

  constructor() {
    effect(() => this.load());
  }

  load(): void {
    this.api.coursePlayer(+this.id()).subscribe((p) => this.player.set(p));
  }

  async feedback(existing?: CourseFeedback): Promise<void> {
    const ref = this.modal.open(FeedbackModalComponent, { centered: true, size: 'lg' });
    const instance = ref.componentInstance as FeedbackModalComponent;
    instance.courseId = +this.id();
    instance.feedback = existing;
    if (await ref.result.catch(() => false)) {
      this.notify.success(existing ? 'Feedback updated successfully.' : 'Feedback added successfully.');
      this.load();
    }
  }

  async deleteFeedback(id: number): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await firstValueFrom(this.api.deleteFeedback(id));
    this.load();
  }
}
