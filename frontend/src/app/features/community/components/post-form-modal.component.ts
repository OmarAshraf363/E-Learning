import { Component, inject, OnInit, signal } from '@angular/core';
import { FormArray, FormBuilder, FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { FormErrorsComponent } from '../../../shared/components/form-errors.component';
import { applyServerErrors } from '../../../shared/forms.util';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { CommunityApi } from '../data-access/community.api';
import { Post } from '../data-access/community.models';

/** Legacy Community/UpsertPost modal: content, key words and an image (with preview). */
@Component({
  selector: 'app-post-form-modal',
  imports: [ReactiveFormsModule, FormErrorsComponent, FileUrlPipe],
  styleUrl: './post-form-modal.component.scss',
  template: `
    <div class="modal-header">
      <h4 class="modal-title text-center mb-3">{{ post ? 'Edit Post' : 'Create New Post' }}</h4>
      <button type="button" class="btn-close" aria-label="Close" (click)="modal.dismiss()"></button>
    </div>
    <div class="modal-body">
      <form [formGroup]="form" (ngSubmit)="save()" novalidate>
        @for (message of generalErrors(); track message) { <div class="alert alert-danger">{{ message }}</div> }
        <div class="form-group mb-3">
          <label for="post-content" class="form-label fw-bold">Content</label>
          <textarea id="post-content" formControlName="content" class="form-control" rows="4" placeholder="Write your post here..."></textarea>
          <app-form-errors [control]="form.controls.content" />
        </div>

        <div class="form-group mb-4">
          <label class="form-label fw-bold me-2">Key Word</label>
          <button type="button" class="btn btn-outline-info btn-sm" (click)="addKeyWord()"><i class="fa-solid fa-plus"></i> Add Key Word</button>
          <div class="mt-3">
            @for (control of form.controls.keyWords.controls; track control; let i = $index) {
              <div class="input-group mb-2">
                <input [formControl]="control" class="form-control" placeholder="Enter word" [attr.aria-label]="'Key word ' + (i + 1)" />
                <button type="button" class="btn btn-outline-danger" (click)="form.controls.keyWords.removeAt(i)"><i class="fas fa-times"></i></button>
              </div>
            }
          </div>
        </div>

        <div class="file-upload">
          <label class="file-upload-btn btn btn-success w-100 mb-2" for="post-image">Add Image</label>
          <input id="post-image" class="d-none" type="file" accept="image/*" (change)="onFile($event)" />
          @if (preview()) {
            <div class="file-upload-content d-block text-center">
              <img class="file-upload-image" [src]="preview()" alt="your image" style="max-width: 100%; max-height: 250px" />
              <div class="image-title-wrap">
                <button type="button" class="remove-image btn btn-sm btn-outline-danger" (click)="removeImage()"><i class="fa fa-trash-can"></i></button>
              </div>
            </div>
          } @else if (post?.attachment) {
            <img [src]="post!.attachment | fileUrl" alt="current image" class="d-block mx-auto mb-2" style="max-height: 200px" />
          }
          <app-form-errors [control]="form.controls.attachment" />
        </div>

        <div class="form-group text-center mt-3">
          <button type="submit" class="btn btn-primary w-50" [disabled]="saving()">{{ post ? 'Update Post' : 'Create Post' }}</button>
        </div>
      </form>
    </div>
  `,
})
export class PostFormModalComponent implements OnInit {
  readonly modal = inject(NgbActiveModal);
  private readonly api = inject(CommunityApi);
  private readonly fb = inject(FormBuilder).nonNullable;

  post?: Post;
  communityId: number | null = null;

  readonly saving = signal(false);
  readonly preview = signal<string | null>(null);
  readonly generalErrors = signal<string[]>([]);
  readonly form = this.fb.group({
    content: ['', Validators.required],
    keyWords: this.fb.array<FormControl<string>>([]),
    attachment: new FormControl<File | null>(null),
  });

  ngOnInit(): void {
    if (this.post) {
      this.form.patchValue({ content: this.post.content });
      this.post.keyWords.forEach((k) => this.addKeyWord(k));
    }
  }

  addKeyWord(value = ''): void {
    (this.form.controls.keyWords as FormArray<FormControl<string>>).push(this.fb.control(value));
  }

  onFile(event: Event): void {
    const file = (event.target as HTMLInputElement).files?.[0] ?? null;
    this.form.controls.attachment.setValue(file);
    this.preview.set(file ? URL.createObjectURL(file) : null);
  }

  removeImage(): void {
    this.form.controls.attachment.setValue(null);
    this.preview.set(null);
  }

  async save(): Promise<void> {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.saving.set(true);
    const v = this.form.getRawValue();
    const body = { ...v, communityId: this.post?.communityId ?? this.communityId };
    try {
      const saved = await firstValueFrom(this.post ? this.api.updatePost(this.post.id, body) : this.api.createPost(body));
      this.modal.close(saved);
    } catch (err) {
      this.generalErrors.set(applyServerErrors(this.form, err));
    } finally {
      this.saving.set(false);
    }
  }
}
