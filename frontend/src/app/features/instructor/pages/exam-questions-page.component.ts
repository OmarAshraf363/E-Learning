import { Component, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { InstructorApi } from '../data-access/instructor.api';
import { ExamQuestions } from '../data-access/instructor.models';

/** Legacy Instructor/Exam/Questions (and the Questions modal of the curriculum). Route: exams/:id/questions */
@Component({
  selector: 'app-exam-questions-page',
  imports: [RouterLink],
  template: `
    @if (data(); as d) {
      <div class="container mt-4">
        <div class="d-flex justify-content-between align-items-center mb-4">
          <h1>Questions for {{ d.examTitle }}</h1>
          <a [routerLink]="['/instructor/exams', d.examId, 'questions', 'new']" class="btn btn-create btn-success"><i class="fas fa-plus"></i> Create New Question</a>
        </div>
        <div class="row g-3">
          @for (question of d.questions; track question.id; let i = $index) {
            <div class="col-md-6">
              <div class="card h-100">
                <div class="card-header d-flex justify-content-between align-items-center">
                  <span>Question {{ i + 1 }}</span>
                  <a class="btn btn-sm btn-outline-warning" [routerLink]="['/instructor/exams', d.examId, 'questions', question.id]"><i class="fas fa-edit"></i> Edit</a>
                </div>
                <div class="card-body">
                  <h5 class="card-title">{{ question.questionText }}</h5>
                  <h6 class="card-subtitle mb-2 text-muted">Choices:</h6>
                  <ul class="list-group">
                    @for (choice of question.choices; track $index) {
                      <li class="list-group-item" [class.list-group-item-success]="choice.isCorrect" [class.list-group-item-secondary]="!choice.isCorrect">
                        {{ choice.choiceText }} @if (choice.isCorrect) { <span class="badge bg-success ms-2">Correct</span> }
                      </li>
                    } @empty {
                      <p class="text-muted">No choices available for this question.</p>
                    }
                  </ul>
                </div>
              </div>
            </div>
          } @empty {
            <div class="alert alert-info">No questions found for this exam.</div>
          }
        </div>
      </div>
    }
  `,
})
export class ExamQuestionsPageComponent {
  private readonly api = inject(InstructorApi);
  readonly id = input.required<string>();
  readonly data = signal<ExamQuestions | null>(null);

  constructor() {
    effect(() => this.api.questions(+this.id()).subscribe((d) => this.data.set(d)));
  }
}
