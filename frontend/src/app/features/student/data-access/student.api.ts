import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../../../core/api/api-client';
import { CoursePlayer, FeedbackRequest, MyCourse, QuickAccess, QuizQuestion, SectionContent, StudentDashboard } from './student.models';

/** Student-area endpoints (/api/student/...). Course content requires an enrollment. */
@Injectable({ providedIn: 'root' })
export class StudentApi {
  private readonly api = inject(ApiClient);

  dashboard = (): Observable<StudentDashboard> => this.api.get('student/dashboard');
  myCourses = (): Observable<MyCourse[]> => this.api.get('student/courses');
  coursePlayer = (courseId: number): Observable<CoursePlayer> => this.api.get(`student/courses/${courseId}`);
  quickAccess = (courseId: number): Observable<QuickAccess> => this.api.get(`student/courses/${courseId}/quick-access`);
  section = (sectionId: number): Observable<SectionContent> => this.api.get(`student/sections/${sectionId}`);
  quiz = (sectionId: number): Observable<QuizQuestion[]> => this.api.get(`student/sections/${sectionId}/quiz`);

  addFeedback = (body: FeedbackRequest): Observable<{ id: number }> => this.api.post('student/feedback', body);
  updateFeedback = (id: number, body: FeedbackRequest): Observable<void> => this.api.put(`student/feedback/${id}`, body);
  deleteFeedback = (id: number): Observable<void> => this.api.delete(`student/feedback/${id}`);

  submitAssignment = (assignmentId: number, content: string): Observable<void> =>
    this.api.post(`student/assignments/${assignmentId}/submissions`, { content });
}
