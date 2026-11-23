import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../../../core/api/api-client';
import { PagedResult } from '../../../core/models/paged-result';
import { CourseFormValue, CourseRequest } from '../../../shared/course-form/course-form.models';
import {
  Assignment, Choice, CourseCurriculum, SectionDeleteSummary, CourseStudent, CurriculumExam, CurriculumReference, CurriculumSection, CurriculumVideo,
  Exam, ExamQuestions, ExamRow, InstructorCourseRow, InstructorDashboard, Option, Question, SectionAssignments, Submission,
} from './instructor.models';

/** All instructor-area endpoints (/api/instructor/...). Ownership is checked by the API. */
@Injectable({ providedIn: 'root' })
export class InstructorApi {
  private readonly api = inject(ApiClient);

  // Dashboard & courses
  dashboard = (): Observable<InstructorDashboard> => this.api.get('instructor/dashboard');
  courses = (page: number): Observable<PagedResult<InstructorCourseRow>> => this.api.get('instructor/courses', { page });
  courseLookup = (): Observable<Option[]> => this.api.get('instructor/courses/lookup');
  formLookups = (): Observable<{ departments: Option[]; tracks: Option[] }> => this.api.get('instructor/courses/form-lookups');
  course = (id: number): Observable<CourseFormValue> => this.api.get(`instructor/courses/${id}`);
  createCourse = (body: CourseRequest): Observable<CourseFormValue> => this.api.postForm('instructor/courses', body);
  updateCourse = (id: number, body: CourseRequest): Observable<CourseFormValue> => this.api.putForm(`instructor/courses/${id}`, body);

  // Curriculum
  curriculum = (courseId: number): Observable<CourseCurriculum> => this.api.get(`instructor/courses/${courseId}/curriculum`);
  addSection = (courseId: number, body: { title: string; content: string }): Observable<CurriculumSection> =>
    this.api.post(`instructor/courses/${courseId}/sections`, body);
  updateSection = (id: number, body: { title: string; content: string }): Observable<void> => this.api.put(`instructor/sections/${id}`, body);
  uploadVideo = (sectionId: number, body: { title: string; video: File | null }): Observable<CurriculumVideo> =>
    this.api.postForm(`instructor/sections/${sectionId}/videos`, body);
  /** The file is replaced only when a new one is chosen. */
  updateVideo = (id: number, body: { title: string; video: File | null }): Observable<CurriculumVideo> =>
    this.api.putForm(`instructor/videos/${id}`, body);
  deleteVideo = (id: number): Observable<void> => this.api.delete(`instructor/videos/${id}`);
  moveSection = (id: number, direction: 'up' | 'down'): Observable<CourseCurriculum> =>
    this.api.put(`instructor/sections/${id}/move`, { direction });
  sectionDeleteSummary = (id: number): Observable<SectionDeleteSummary> => this.api.get(`instructor/sections/${id}/delete-summary`);
  deleteSection = (id: number): Observable<void> => this.api.delete(`instructor/sections/${id}`);
  addReference = (sectionId: number, body: { resourceTitle: string; resourceURL: string }): Observable<CurriculumReference> =>
    this.api.post(`instructor/sections/${sectionId}/references`, body);
  updateReference = (id: number, body: { resourceTitle: string; resourceURL: string }): Observable<void> =>
    this.api.put(`instructor/references/${id}`, body);
  deleteReference = (id: number): Observable<void> => this.api.delete(`instructor/references/${id}`);
  addQuiz = (sectionId: number, body: { title: string; examDate: string }): Observable<CurriculumExam> =>
    this.api.post(`instructor/sections/${sectionId}/quizzes`, body);
  updateQuiz = (id: number, body: { title: string; examDate: string }): Observable<void> => this.api.put(`instructor/quizzes/${id}`, body);
  deleteQuiz = (id: number): Observable<void> => this.api.delete(`instructor/quizzes/${id}`);

  // Exams & questions
  exams = (): Observable<ExamRow[]> => this.api.get('instructor/exams');
  exam = (id: number): Observable<Exam> => this.api.get(`instructor/exams/${id}`);
  createExam = (body: Omit<Exam, 'id'>): Observable<Exam> => this.api.post('instructor/exams', body);
  updateExam = (id: number, body: Omit<Exam, 'id'>): Observable<Exam> => this.api.put(`instructor/exams/${id}`, body);
  deleteExam = (id: number): Observable<void> => this.api.delete(`instructor/exams/${id}`);
  questions = (examId: number): Observable<ExamQuestions> => this.api.get(`instructor/exams/${examId}/questions`);
  question = (id: number): Observable<Question> => this.api.get(`instructor/questions/${id}`);
  addQuestion = (examId: number, body: { questionText: string; choices: Choice[] }): Observable<Question> =>
    this.api.post(`instructor/exams/${examId}/questions`, body);
  updateQuestion = (id: number, body: { questionText: string; choices: Choice[] }): Observable<Question> =>
    this.api.put(`instructor/questions/${id}`, body);

  // Assignments
  assignmentsByCourse = (courseId: number): Observable<SectionAssignments[]> => this.api.get(`instructor/courses/${courseId}/assignments`);
  assignment = (id: number): Observable<Assignment> => this.api.get(`instructor/assignments/${id}`);
  createAssignment = (sectionId: number, body: AssignmentBody): Observable<Assignment> =>
    this.api.postForm(`instructor/sections/${sectionId}/assignments`, body);
  updateAssignment = (id: number, body: AssignmentBody): Observable<Assignment> => this.api.putForm(`instructor/assignments/${id}`, body);
  deleteAssignment = (id: number): Observable<void> => this.api.delete(`instructor/assignments/${id}`);
  submissions = (id: number): Observable<Submission[]> => this.api.get(`instructor/assignments/${id}/submissions`);

  // Students
  students = (courseId: number): Observable<CourseStudent[]> => this.api.get(`instructor/courses/${courseId}/students`);
  addFeedback = (body: { studentId: string; content: string }): Observable<void> => this.api.post('instructor/feedback', body);
}

export interface AssignmentBody {
  title: string;
  description: string | null;
  deadLine: string;
  content: File | null;
}
