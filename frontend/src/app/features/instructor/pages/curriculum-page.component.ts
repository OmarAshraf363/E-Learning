import { DatePipe } from '@angular/common';
import { Component, effect, inject, input, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { openFormModal } from '../../../shared/components/form-modal.component';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { AssignmentBody, InstructorApi } from '../data-access/instructor.api';
import {
  CourseCurriculum, CurriculumAssignment, CurriculumExam, CurriculumReference, CurriculumSection,
} from '../data-access/instructor.models';

/**
 * Curriculum manager (legacy _CourseCurriculumPartial): sections with videos, references,
 * assignments and quizzes. Route: /instructor/courses/:id/curriculum
 */
@Component({
  selector: 'app-curriculum-page',
  imports: [RouterLink, DatePipe, FileUrlPipe],
  templateUrl: './curriculum-page.component.html',
  styleUrl: './curriculum-page.component.scss',
})
export class CurriculumPageComponent {
  private readonly api = inject(InstructorApi);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);

  readonly id = input.required<string>();
  readonly curriculum = signal<CourseCurriculum | null>(null);

  constructor() {
    effect(() => this.load());
  }

  load(): void {
    this.api.curriculum(+this.id()).subscribe((c) => this.curriculum.set(c));
  }

  // ---- sections
  async editSection(section?: CurriculumSection): Promise<void> {
    await this.openAndReload<{ title: string; content: string }>({
      title: section ? 'Edit Curriculum' : 'Add New Curriculum',
      fields: [
        { name: 'title', label: 'Title', required: true },
        { name: 'content', label: 'Content', type: 'textarea', required: true },
      ],
      value: section,
      submit: (v) => firstValueFrom<unknown>(section ? this.api.updateSection(section.id, v) : this.api.addSection(+this.id(), v)),
    });
  }

  // ---- videos
  async addVideo(section: CurriculumSection): Promise<void> {
    await this.openAndReload<{ title: string; video: File | null }>({
      title: 'Add Video',
      fields: [
        { name: 'title', label: 'Title', required: true },
        { name: 'video', label: 'Video', type: 'file', accept: 'video/*', required: true },
      ],
      submit: (v) => firstValueFrom(this.api.uploadVideo(section.id, v)),
    });
  }

  async deleteVideo(id: number): Promise<void> {
    await this.confirmAndReload(() => this.api.deleteVideo(id));
  }

  // ---- references
  async editReference(section: CurriculumSection, reference?: CurriculumReference): Promise<void> {
    await this.openAndReload<{ resourceTitle: string; resourceURL: string }>({
      title: reference ? 'Edit Reference' : 'Add Reference',
      fields: [
        { name: 'resourceTitle', label: 'ResourceTitle', required: true },
        { name: 'resourceURL', label: 'ResourceURL', type: 'url', required: true },
      ],
      value: reference ? { resourceTitle: reference.title, resourceURL: reference.url } : undefined,
      submit: (v) => firstValueFrom<unknown>(reference ? this.api.updateReference(reference.id, v) : this.api.addReference(section.id, v)),
    });
  }

  async deleteReference(id: number): Promise<void> {
    await this.confirmAndReload(() => this.api.deleteReference(id));
  }

  // ---- assignments
  async editAssignment(section: CurriculumSection, assignment?: CurriculumAssignment): Promise<void> {
    await this.openAndReload<AssignmentBody>({
      title: assignment ? 'Edit Assignment' : 'Create New Assignment',
      fields: [
        { name: 'title', label: 'Title', required: true },
        { name: 'content', label: assignment ? 'File (leave empty to keep the current one)' : 'File', type: 'file', required: !assignment },
        { name: 'deadLine', label: 'DeadLine', type: 'datetime-local', required: true },
        { name: 'description', label: 'Description', type: 'textarea' },
      ],
      value: assignment ? { ...assignment, deadLine: assignment.deadLine.slice(0, 16) } : undefined,
      submit: (v) => firstValueFrom<unknown>(assignment ? this.api.updateAssignment(assignment.id, v) : this.api.createAssignment(section.id, v)),
    });
  }

  async deleteAssignment(id: number): Promise<void> {
    await this.confirmAndReload(() => this.api.deleteAssignment(id));
  }

  // ---- quizzes
  async editQuiz(section: CurriculumSection, exam?: CurriculumExam): Promise<void> {
    await this.openAndReload<{ title: string; examDate: string }>({
      title: exam ? 'Edit Quiz' : 'Add New Quiz',
      fields: [
        { name: 'title', label: 'Title', required: true },
        { name: 'examDate', label: 'ExamDate', type: 'datetime-local', required: true },
      ],
      value: exam ? { title: exam.title, examDate: exam.examDate.slice(0, 16) } : undefined,
      submitText: exam ? 'Save Changes' : 'Create',
      submit: (v) => firstValueFrom<unknown>(exam ? this.api.updateQuiz(exam.id, v) : this.api.addQuiz(section.id, v)),
    });
  }

  async deleteQuiz(id: number): Promise<void> {
    await this.confirmAndReload(() => this.api.deleteQuiz(id));
  }

  // ---- helpers
  private async openAndReload<T>(options: Parameters<typeof openFormModal<T>>[1]): Promise<void> {
    const result = await openFormModal<T>(this.modal, options);
    if (result !== undefined) {
      this.notify.success('Saved successfully');
      this.load();
    }
  }

  private async confirmAndReload(action: () => ReturnType<InstructorApi['deleteVideo']>): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await firstValueFrom(action());
    this.load();
  }
}
