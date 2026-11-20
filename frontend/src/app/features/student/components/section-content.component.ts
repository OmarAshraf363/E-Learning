import { DatePipe } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Component, effect, inject, input, signal } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { ProblemDetails } from '../../../core/models/problem-details';
import { NotifyService } from '../../../core/ui/notify.service';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { StarRatingComponent } from '../../../shared/components/star-rating.component';
import { StudentApi } from '../data-access/student.api';
import { PlayerVideo, SectionContent } from '../data-access/student.models';
import { FeedbackModalComponent } from './feedback-modal.component';
import { QuizModalComponent } from './quiz-modal.component';

/**
 * One curriculum section in the course player (legacy Student/DashBoard/GetContent):
 * videos (quiz pops up after 25% of a video when the section has one), resources,
 * assignment submission and section feedback.
 */
@Component({
  selector: 'app-section-content',
  imports: [DatePipe, FileUrlPipe, StarRatingComponent],
  templateUrl: './section-content.component.html',
  styleUrl: './section-content.component.scss',
})
export class SectionContentComponent {
  private readonly api = inject(StudentApi);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);

  readonly sectionId = input.required<number>();
  readonly section = signal<SectionContent | null>(null);
  readonly video = signal<PlayerVideo | null>(null);
  readonly solutions: Record<number, string> = {};
  private quizShown = false;

  constructor() {
    effect(() => {
      this.video.set(null);
      this.quizShown = false;
      this.load();
    });
  }

  load(): void {
    this.api.section(this.sectionId()).subscribe((s) => this.section.set(s));
  }

  /** Legacy testVideo(): after 25% of the video, show the section quiz once. */
  onTimeUpdate(event: Event): void {
    const player = event.target as HTMLVideoElement;
    if (this.quizShown || !this.section()?.hasQuiz || !player.duration) return;
    if ((player.currentTime / player.duration) * 100 > 25) {
      this.quizShown = true;
      this.api.quiz(this.sectionId()).subscribe((questions) => {
        const ref = this.modal.open(QuizModalComponent, { centered: true, size: 'lg' });
        (ref.componentInstance as QuizModalComponent).questions = questions;
      });
    }
  }

  async submit(assignmentId: number): Promise<void> {
    const content = this.solutions[assignmentId]?.trim();
    if (!content) {
      this.notify.error('Enter your solution URL first.');
      return;
    }
    try {
      await firstValueFrom(this.api.submitAssignment(assignmentId, content));
      this.notify.success('Assignment submitted.');
      this.load();
    } catch (err) {
      if (!(err instanceof HttpErrorResponse)) this.notify.error((err as ProblemDetails)?.detail ?? 'Submission failed.');
    }
  }

  async addFeedback(): Promise<void> {
    const ref = this.modal.open(FeedbackModalComponent, { centered: true, size: 'lg' });
    (ref.componentInstance as FeedbackModalComponent).curriculumId = this.sectionId();
    const saved = await ref.result.catch(() => false);
    if (saved) {
      this.notify.success('Feedback added successfully.');
      this.load();
    }
  }
}
