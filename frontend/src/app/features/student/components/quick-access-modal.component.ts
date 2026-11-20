import { Component, inject, OnInit, signal } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { StudentApi } from '../data-access/student.api';
import { QuickAccess } from '../data-access/student.models';

/** Legacy Student/DashBoard/QuickAccess: every section's videos, references and assignments. */
@Component({
  selector: 'app-quick-access-modal',
  imports: [FileUrlPipe],
  styleUrl: './quick-access-modal.component.scss',
  template: `
    <div class="modal-header bg-dark text-white">
      <h5 class="modal-title">Content</h5>
      <button type="button" class="btn-close btn-close-white" aria-label="Close" (click)="modal.dismiss()"></button>
    </div>
    <div class="modal-body px-4 py-3">
      @if (data(); as d) {
        <ul class="list-group">
          <li class="list-group-item mb-4"><h3>{{ d.courseName }}</h3></li>
          @for (section of d.sections; track $index) {
            <li class="list-group-item mb-4">
              <h5 class="font-weight-bold"># {{ section.title }}</h5>
              <p>{{ section.content }}</p>
              <div class="curriculum-section">
                <h5>Videos</h5>
                @for (video of section.videos; track video.id) {
                  <div class="resource-card"><a [href]="video.url | fileUrl" target="_blank"><i class="fa-solid fa-video"></i> {{ video.title }}</a></div>
                } @empty { <div class="no-data">No videos available for this curriculum.</div> }
              </div>
              <div class="curriculum-section">
                <h5>References</h5>
                @for (ref of section.resources; track ref.id) {
                  <div class="resource-card"><a [href]="ref.url" target="_blank"><i class="fa-solid fa-book"></i> {{ ref.title }}</a></div>
                } @empty { <div class="no-data">No references available for this curriculum.</div> }
              </div>
              <div class="curriculum-section">
                <h5>Assignments</h5>
                @for (a of section.assignments; track a.id) {
                  <div class="resource-card"><a [href]="a.url | fileUrl" target="_blank"><i class="fa-solid fa-file"></i> {{ a.title }}</a></div>
                } @empty { <div class="no-data">No assignments available for this curriculum.</div> }
              </div>
            </li>
          }
        </ul>
      } @else {
        <div class="text-center py-5"><div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading..</span></div></div>
      }
    </div>
    <div class="modal-footer bg-light"><button type="button" class="btn btn-secondary" (click)="modal.dismiss()">Close</button></div>
  `,
})
export class QuickAccessModalComponent implements OnInit {
  readonly modal = inject(NgbActiveModal);
  private readonly api = inject(StudentApi);

  courseId!: number;
  readonly data = signal<QuickAccess | null>(null);

  ngOnInit(): void {
    this.api.quickAccess(this.courseId).subscribe((d) => this.data.set(d));
  }
}
