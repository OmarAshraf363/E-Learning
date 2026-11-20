import { DatePipe } from '@angular/common';
import { Component, effect, inject, input, signal, TemplateRef, viewChild } from '@angular/core';
import { toSignal } from '@angular/core/rxjs-interop';
import { Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { openFormModal } from '../../../shared/components/form-modal.component';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { AssignmentBody, InstructorApi } from '../data-access/instructor.api';
import { Assignment, SectionAssignments, Submission } from '../data-access/instructor.models';

/** Legacy Instructor/Assinment/Index (+ details, submissions and upsert modals). ?courseId= selects the course. */
@Component({
  selector: 'app-assignments-page',
  imports: [DatePipe, FileUrlPipe],
  templateUrl: './assignments-page.component.html',
})
export class AssignmentsPageComponent {
  private readonly api = inject(InstructorApi);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);
  private readonly router = inject(Router);
  private readonly detailsTpl = viewChild.required<TemplateRef<unknown>>('detailsModal');
  private readonly submissionsTpl = viewChild.required<TemplateRef<unknown>>('submissionsModal');

  readonly courseId = input<string>();
  readonly courses = toSignal(this.api.courseLookup(), { initialValue: [] });
  readonly sections = signal<SectionAssignments[]>([]);
  readonly selected = signal<Assignment | null>(null);
  readonly submissions = signal<Submission[]>([]);

  constructor() {
    effect(() => this.load());
  }

  load(): void {
    const id = this.courseId();
    if (id) this.api.assignmentsByCourse(+id).subscribe((s) => this.sections.set(s));
    else this.sections.set([]);
  }

  selectCourse(value: string): void {
    void this.router.navigate([], { queryParams: { courseId: value || null } });
  }

  openDetails(assignment: Assignment): void {
    this.selected.set(assignment);
    this.modal.open(this.detailsTpl(), { centered: true });
  }

  openSubmissions(assignment: Assignment): void {
    this.submissions.set([]);
    this.api.submissions(assignment.id).subscribe((s) => this.submissions.set(s));
    this.modal.open(this.submissionsTpl(), { size: 'xl', centered: true });
  }

  async upsert(sectionId: number, assignment?: Assignment): Promise<void> {
    this.modal.dismissAll();
    const result = await openFormModal<AssignmentBody>(this.modal, {
      title: assignment ? 'Edit Assignment' : 'Create New Assignment',
      fields: [
        { name: 'title', label: 'Title', required: true },
        { name: 'content', label: assignment ? 'File (leave empty to keep the current one)' : 'File', type: 'file', required: !assignment },
        { name: 'deadLine', label: 'DeadLine', type: 'datetime-local', required: true },
        { name: 'description', label: 'Description', type: 'textarea' },
      ],
      value: assignment ? { ...assignment, deadLine: assignment.deadLine.slice(0, 16) } : undefined,
      submit: (v) => firstValueFrom<unknown>(assignment ? this.api.updateAssignment(assignment.id, v) : this.api.createAssignment(sectionId, v)),
    });
    if (result !== undefined) {
      this.notify.success('Saved successfully');
      this.load();
    }
  }

  async remove(assignment: Assignment): Promise<void> {
    this.modal.dismissAll();
    if (!(await this.notify.confirm())) return;
    await firstValueFrom(this.api.deleteAssignment(assignment.id));
    this.load();
  }
}
