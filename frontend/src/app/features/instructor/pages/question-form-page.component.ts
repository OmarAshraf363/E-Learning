import { Component, inject, input, OnInit, signal } from '@angular/core';
import {
  AbstractControl, FormArray, FormBuilder, FormGroup, FormControl, ReactiveFormsModule, ValidationErrors, Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { FormErrorsComponent } from '../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../shared/forms.util';
import { InstructorApi } from '../data-access/instructor.api';

type ChoiceGroup = FormGroup<{ choiceText: FormControl<string>; isCorrect: FormControl<boolean> }>;

/**
 * Business rule (second plan): at least two choices and at least one correct answer
 * (several correct answers are allowed). Empty choices are ignored, as on the server.
 */
function choicesRule(control: AbstractControl): ValidationErrors | null {
  const filled = (control.value as { choiceText: string; isCorrect: boolean }[]).filter((c) => c.choiceText.trim());
  if (filled.length < 2) return { minChoices: 'Please add at least two choices.' };
  if (!filled.some((c) => c.isCorrect)) return { correctChoice: 'Please mark at least one choice as correct.' };
  return null;
}

/** Legacy Instructor/Exam/UpSertQuestions. Routes: exams/:id/questions/new and exams/:id/questions/:questionId */
@Component({
  selector: 'app-question-form-page',
  imports: [ReactiveFormsModule, RouterLink, FormErrorsComponent],
  template: `
    <div class="container mt-4" style="max-width: 800px">
      <h1>{{ questionId() ? 'Edit Question' : 'Add Question' }}</h1>
      <form [formGroup]="form" (ngSubmit)="save()" novalidate>
        <div class="form-group mb-3">
          <label for="questionText" class="control-label">QuestionText</label>
          <input id="questionText" formControlName="questionText" class="form-control" />
          <app-form-errors [control]="form.controls.questionText" />
        </div>

        <h4>Choices</h4>
        <p class="text-muted small">Add at least two choices and mark the correct answer (more than one can be correct).</p>
        @for (choice of form.controls.choices.controls; track choice; let i = $index) {
          <div class="d-flex align-items-center gap-2 mb-2" [formGroup]="choice">
            <label class="text-nowrap">Choice {{ i + 1 }}</label>
            <input type="text" formControlName="choiceText" class="form-control" />
            <div class="form-check text-nowrap">
              <input type="checkbox" formControlName="isCorrect" class="form-check-input" [id]="'correct-' + i" />
              <label class="form-check-label" [for]="'correct-' + i">Correct</label>
            </div>
            <button type="button" class="btn btn-outline-danger btn-sm" (click)="form.controls.choices.removeAt(i)"><i class="fas fa-times"></i></button>
          </div>
        }

        <app-form-errors [control]="form.controls.choices" />

        <div class="d-flex gap-2 mt-3">
          <button type="button" class="btn btn-secondary" (click)="addChoice()">Add Choice</button>
          <button type="submit" class="btn btn-primary" [disabled]="saving()">Save</button>
          <a [routerLink]="['/instructor/exams', id(), 'questions']" class="btn btn-secondary">Cancel</a>
        </div>
      </form>
    </div>
  `,
})
export class QuestionFormPageComponent implements OnInit {
  private readonly api = inject(InstructorApi);
  private readonly router = inject(Router);
  private readonly notify = inject(NotifyService);
  private readonly fb = inject(FormBuilder).nonNullable;

  /** Exam id */
  readonly id = input.required<string>();
  readonly questionId = input<string>();
  readonly saving = signal(false);

  readonly form = this.fb.group({
    questionText: ['', Validators.required],
    choices: this.fb.array<ChoiceGroup>([], choicesRule),
  });

  ngOnInit(): void {
    const questionId = this.questionId();
    if (questionId) {
      this.api.question(+questionId).subscribe((q) => {
        this.form.patchValue({ questionText: q.questionText });
        q.choices.forEach((c) => this.addChoice(c.choiceText, c.isCorrect));
      });
    } else {
      this.addChoice();
      this.addChoice();
    }
  }

  addChoice(choiceText = '', isCorrect = false): void {
    (this.form.controls.choices as FormArray<ChoiceGroup>).push(
      this.fb.group({ choiceText: [choiceText], isCorrect: [isCorrect] }),
    );
  }

  async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const body = this.form.getRawValue();
    try {
      const questionId = this.questionId();
      await firstValueFrom(questionId ? this.api.updateQuestion(+questionId, body) : this.api.addQuestion(+this.id(), body));
      this.notify.success('Question saved');
      void this.router.navigate(['/instructor/exams', this.id(), 'questions']);
    } catch (err) {
      applyServerErrors(this.form, err);
    } finally {
      this.saving.set(false);
    }
  }
}
