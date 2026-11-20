import { DatePipe } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { InstructorApi } from '../data-access/instructor.api';
import { ExamRow } from '../data-access/instructor.models';

/** Legacy Instructor/Exam/Index. */
@Component({
  selector: 'app-exams-page',
  imports: [RouterLink, DatePipe],
  template: `
    <div class="container mt-4">
      <div class="d-flex justify-content-between align-items-center">
        <div>
          <h1 class="display-4">My Exams</h1>
          <p class="lead">Here is a list of exams that you have created.</p>
        </div>
        <a routerLink="/instructor/exams/new" class="btn btn-dark"><i class="fas fa-plus"></i> New Exam</a>
      </div>
      @if (exams().length) {
        <table class="table table-hover table-bordered">
          <thead class="thead-dark"><tr><th>Exam Title</th><th>Course</th><th>Exam Date</th><th>Total Questions</th><th>Actions</th></tr></thead>
          <tbody>
            @for (exam of exams(); track exam.id) {
              <tr>
                <td>{{ exam.title }}</td>
                <td>{{ exam.courseName }}</td>
                <td>{{ exam.examDate | date: 'dd MMM yyyy' }}</td>
                <td>{{ exam.totalQuestions }}</td>
                <td class="d-flex gap-1">
                  <a [routerLink]="['/instructor/exams', exam.id, 'questions']" class="btn btn-info btn-sm"><i class="fas fa-eye"></i> View</a>
                  <a [routerLink]="['/instructor/exams', exam.id]" class="btn btn-warning btn-sm"><i class="fas fa-edit"></i> Edit</a>
                  <button type="button" class="btn btn-danger btn-sm" (click)="remove(exam.id)"><i class="fas fa-trash"></i> Delete</button>
                </td>
              </tr>
            }
          </tbody>
        </table>
      } @else {
        <div class="alert alert-info">No exams found. <a routerLink="/instructor/exams/new" class="alert-link">Create a new exam</a>.</div>
      }
    </div>
  `,
})
export class ExamsPageComponent {
  private readonly api = inject(InstructorApi);
  private readonly notify = inject(NotifyService);
  readonly exams = signal<ExamRow[]>([]);

  constructor() {
    this.load();
  }

  load(): void {
    this.api.exams().subscribe((e) => this.exams.set(e));
  }

  async remove(id: number): Promise<void> {
    if (!(await this.notify.confirm())) return;
    await firstValueFrom(this.api.deleteExam(id));
    this.load();
  }
}
