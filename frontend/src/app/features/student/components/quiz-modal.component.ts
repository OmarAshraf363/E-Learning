import { Component, computed, inject, signal } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { QuizQuestion } from '../data-access/student.models';

/**
 * Section quiz shown after 25% of a video (legacy loadQuestions / GetQuestion stepper).
 * Answers are not graded or saved: exam submission did not exist in the legacy app (backlog).
 */
@Component({
  selector: 'app-quiz-modal',
  styleUrl: './quiz-modal.component.scss',
  template: `
    <div class="modal-header bg-dark text-white">
      <h5 class="modal-title">Quick Quiz</h5>
      <button type="button" class="btn-close btn-close-white" aria-label="Close" (click)="modal.dismiss()"></button>
    </div>
    <div class="modal-body">
      @if (current(); as q) {
        <div class="question-container">
          <h4>{{ q.questionText }}</h4>
          <ul class="list-unstyled">
            @for (choice of q.choices; track $index) {
              <li><label><input type="checkbox" class="me-2" /> {{ choice }}</label></li>
            }
          </ul>
          <div class="d-flex justify-content-between">
            <button type="button" class="btn btn-secondary" [disabled]="index() === 0" (click)="index.set(index() - 1)">Previous Question</button>
            <button type="button" class="btn btn-primary" (click)="next()">{{ isLast() ? 'Finish' : 'Next Question' }}</button>
          </div>
        </div>
      } @else {
        <div>No Found Questions</div>
      }
    </div>
  `,
})
export class QuizModalComponent {
  readonly modal = inject(NgbActiveModal);

  questions: QuizQuestion[] = [];
  readonly index = signal(0);
  readonly current = computed(() => this.questions[this.index()]);
  readonly isLast = computed(() => this.index() === this.questions.length - 1);

  next(): void {
    if (this.isLast()) this.modal.close();
    else this.index.set(this.index() + 1);
  }
}
