import { DatePipe } from '@angular/common';
import { Component, inject, OnInit, signal } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { CommunityApi } from '../data-access/community.api';
import { Comment } from '../data-access/community.models';
import { ReactionsModalComponent } from './reactions-modal.component';

/**
 * Legacy GetComments / GetReplies modal: comments with "Love" reaction, reply box and replies.
 * Set postId (comments of a post) or parentCommentId (replies of a comment).
 */
@Component({
  selector: 'app-comments-modal',
  imports: [DatePipe, FileUrlPipe],
  styleUrl: './comments-modal.component.scss',
  template: `
    <div class="modal-header">
      <h5 class="modal-title">{{ parentCommentId ? 'Replies' : 'Comments' }}</h5>
      <button type="button" class="btn-close" aria-label="Close" (click)="modal.close(changed)"></button>
    </div>
    <div class="modal-body comments-container">
      @for (comment of comments(); track comment.id) {
        <div class="comment-item">
          <div class="comment-header">
            <img [src]="comment.author.picture | fileUrl: 'images/default-avatar.svg'" [alt]="comment.author.fullName" />
            <div class="user-details">
              <span class="name">{{ comment.author.fullName }}</span>
              <span class="timestamp">{{ comment.createdAt | date: 'short' }}</span>
            </div>
          </div>
          <div class="comment-content"><p>{{ comment.content }}</p></div>
          <div class="post-reaction text-black d-flex gap-3">
            <span role="button" (click)="showReactions(comment.id)"><span>{{ comment.reactionsCount }}</span> Reactions</span>
            @if (!parentCommentId && comment.repliesCount) {
              <span role="button" (click)="openReplies(comment)">{{ comment.repliesCount }} Replies</span>
            }
          </div>
          @if (!parentCommentId) {
            <div class="comment-actions">
              <button type="button" class="react-btn" (click)="react(comment)">
                <i class="fa fa-heart" [class.fa-solid]="comment.reactedByMe" [class.fa-regular]="!comment.reactedByMe"></i> Love
              </button>
              <button type="button" (click)="replyOpen.set(replyOpen() === comment.id ? null : comment.id)"><i class="fa fa-reply"></i> Reply</button>
            </div>
            @if (replyOpen() === comment.id) {
              <div class="reply-box">
                <textarea #reply class="form-control mb-2" placeholder="Write a reply..." [attr.aria-label]="'Reply to ' + comment.author.fullName"></textarea>
                <button type="button" class="btn btn-primary btn-sm" (click)="submitReply(comment, reply.value); reply.value = ''">Post Reply</button>
              </div>
            }
          }
        </div>
      } @empty {
        <p class="no-comments">No comments yet. Be the first to comment!</p>
      }
    </div>
  `,
})
export class CommentsModalComponent implements OnInit {
  readonly modal = inject(NgbActiveModal);
  private readonly api = inject(CommunityApi);
  private readonly modals = inject(NgbModal);

  postId?: number;
  parentCommentId?: number;
  changed = false;

  readonly comments = signal<Comment[]>([]);
  readonly replyOpen = signal<number | null>(null);

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    const source = this.parentCommentId ? this.api.replies(this.parentCommentId) : this.api.comments(this.postId!);
    source.subscribe((c) => this.comments.set(c));
  }

  async react(comment: Comment): Promise<void> {
    const result = await firstValueFrom(this.api.reactToComment(comment.id));
    this.comments.update((list) =>
      list.map((c) => (c.id === comment.id ? { ...c, reactedByMe: result.state === 'Add', reactionsCount: result.count } : c)),
    );
  }

  async submitReply(comment: Comment, content: string): Promise<void> {
    if (!content.trim()) return;
    await firstValueFrom(this.api.addReply(comment.id, content));
    this.changed = true;
    this.replyOpen.set(null);
    this.load();
  }

  showReactions(commentId: number): void {
    const ref = this.modals.open(ReactionsModalComponent, { centered: true });
    (ref.componentInstance as ReactionsModalComponent).source = this.api.commentReactions(commentId);
  }

  openReplies(comment: Comment): void {
    const ref = this.modals.open(CommentsModalComponent, { centered: true, size: 'lg', scrollable: true });
    (ref.componentInstance as CommentsModalComponent).parentCommentId = comment.id;
  }
}
