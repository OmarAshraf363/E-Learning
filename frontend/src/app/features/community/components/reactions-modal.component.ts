import { Component, inject, OnInit, signal } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { Reaction } from '../data-access/community.models';

/** Legacy GetReaction / GetCommentReaction: who reacted and how. Set `source` before opening. */
@Component({
  selector: 'app-reactions-modal',
  imports: [FileUrlPipe],
  styleUrl: './reactions-modal.component.scss',
  template: `
    <div class="modal-header">
      <h5 class="modal-title">Reactions</h5>
      <button type="button" class="btn-close" aria-label="Close" (click)="modal.dismiss()"></button>
    </div>
    <div class="modal-body reaction-list">
      @for (r of reactions(); track $index) {
        <div class="reaction-item">
          <img [src]="r.userPicture | fileUrl: 'images/default-avatar.svg'" [alt]="r.userFullName" class="user-image" />
          <div class="user-details">
            <span class="user-name">{{ r.userFullName }}</span>
            <span class="reaction-type">Reacted: <strong>{{ r.type }}</strong></span>
          </div>
        </div>
      } @empty {
        <p class="no-reactions">No reactions yet. Be the first to react!</p>
      }
    </div>
  `,
})
export class ReactionsModalComponent implements OnInit {
  readonly modal = inject(NgbActiveModal);
  source!: Observable<Reaction[]>;
  readonly reactions = signal<Reaction[]>([]);

  ngOnInit(): void {
    this.source.subscribe((r) => this.reactions.set(r));
  }
}
