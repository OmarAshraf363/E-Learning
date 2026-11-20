import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { FormErrorsComponent } from '../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../shared/forms.util';
import { StudentApi } from '../data-access/student.api';
import { CourseFeedback } from '../data-access/student.models';

/**
 * Legacy Student/DashBoard/UpsertFeedback modal: star rating + review.
 * Set courseId (course feedback) or curriculumId (section feedback); set feedback to edit.
 */
@Component({
  selector: 'app-feedback-modal',
  imports: [ReactiveFormsModule, FormErrorsComponent],
  styleUrl: './feedback-modal.component.scss',
  template: `
    <div class="modal-header bg-dark text-white">
      <h5 class="modal-title">{{ feedback ? 'Edit Feedback' : 'Feedback' }}</h5>
      <button type="button" class="btn-close btn-close-white" aria-label="Close" (click)="modal.dismiss()"></button>
    </div>
    <div class="modal-body px-4 py-3">
      <form [formGroup]="form" (ngSubmit)="save()" novalidate>
        <div class="w-100 mb-3 d-flex justify-content-center align-items-center">
          <div class="star-rating">
            @for (star of [5, 4, 3, 2, 1]; track star) {
              <input type="radio" [id]="'star-' + star" name="rating" [value]="star" formControlName="rating" />
              <label [for]="'star-' + star" [title]="star + ' stars'">&#9733;</label>
            }
          </div>
        </div>
        <div class="mb-3">
          <label for="content" class="form-label">Review</label>
          <textarea id="content" formControlName="content" class="form-control" rows="4" placeholder="Write your feedback..."></textarea>
          <app-form-errors [control]="form.controls.content" />
        </div>
        @for (message of generalErrors(); track message) { <div class="text-danger mb-2">{{ message }}</div> }
        <button type="submit" class="w-100 btn btn-dark" [disabled]="saving()">Submit</button>
      </form>
    </div>
  `,
})
export class FeedbackModalComponent implements OnInit {
  readonly modal = inject(NgbActiveModal);
  private readonly api = inject(StudentApi);

  courseId: number | null = null;
  curriculumId: number | null = null;
  feedback?: CourseFeedback;

  readonly saving = signal(false);
  readonly generalErrors = signal<string[]>([]);
  readonly form = inject(FormBuilder).group({
    rating: [null as number | null],
    content: ['', Validators.required],
  });

  ngOnInit(): void {
    if (this.feedback) this.form.patchValue({ rating: this.feedback.rating, content: this.feedback.content });
  }

  async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const v = this.form.getRawValue();
    const body = { courseId: this.courseId, courseCurriculumId: this.curriculumId, rating: v.rating ? +v.rating : null, content: v.content! };
    try {
      await firstValueFrom<unknown>(this.feedback ? this.api.updateFeedback(this.feedback.id, body) : this.api.addFeedback(body));
      this.modal.close(true);
    } catch (err) {
      this.generalErrors.set(applyServerErrors(this.form, err));
    } finally {
      this.saving.set(false);
    }
  }
}
