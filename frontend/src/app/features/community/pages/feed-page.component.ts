import { DatePipe, Location } from '@angular/common';
import { Component, effect, inject, input, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { firstValueFrom } from 'rxjs';
import { NotifyService } from '../../../core/ui/notify.service';
import { FileUrlPipe } from '../../../shared/pipes/file-url.pipe';
import { CommentsModalComponent } from '../components/comments-modal.component';
import { PostFormModalComponent } from '../components/post-form-modal.component';
import { ReactionsModalComponent } from '../components/reactions-modal.component';
import { CommunityApi } from '../data-access/community.api';
import { Feed, Post } from '../data-access/community.models';

/**
 * Legacy Customer/Community/Index (LinkedIn-like feed).
 * Filters are query params: ?id= (community) &word= &maxReact=true &userName= &userId=
 */
@Component({
  selector: 'app-feed-page',
  imports: [RouterLink, FormsModule, DatePipe, FileUrlPipe],
  templateUrl: './feed-page.component.html',
  styleUrl: './feed-page.component.scss',
})
export class FeedPageComponent {
  private readonly api = inject(CommunityApi);
  private readonly modal = inject(NgbModal);
  private readonly notify = inject(NotifyService);
  private readonly router = inject(Router);
  readonly location = inject(Location);

  // query params
  readonly id = input<string>();
  readonly word = input<string>();
  readonly maxReact = input<string>();
  readonly userName = input<string>();
  readonly userId = input<string>();

  readonly feed = signal<Feed | null>(null);
  readonly openMenu = signal<number | null>(null);
  readonly commentDrafts: Record<number, string> = {};
  wordSearch = '';
  userSearch = '';

  constructor() {
    effect(() => {
      this.wordSearch = this.word() ?? '';
      this.userSearch = this.userName() ?? '';
      this.load();
    });
  }

  load(): void {
    this.api
      .feed({
        id: this.id() ? +this.id()! : null,
        word: this.word(),
        maxReact: this.maxReact() === 'true' ? true : null,
        userName: this.userName(),
        userId: this.userId(),
      })
      .subscribe((f) => this.feed.set(f));
  }

  /** Keeps the community / selected user while changing the other filters. */
  filter(changes: Record<string, string | boolean | null>): void {
    void this.router.navigate([], {
      queryParams: { id: this.id() ?? null, userId: this.userId() ?? null, ...changes },
    });
  }

  async openPostForm(post?: Post): Promise<void> {
    this.openMenu.set(null);
    const ref = this.modal.open(PostFormModalComponent, { size: 'lg', centered: true, scrollable: true });
    const instance = ref.componentInstance as PostFormModalComponent;
    instance.post = post;
    instance.communityId = this.id() ? +this.id()! : null;
    if (await ref.result.catch(() => null)) this.load();
  }

  async deletePost(post: Post): Promise<void> {
    this.openMenu.set(null);
    if (!(await this.notify.confirm())) return;
    await firstValueFrom(this.api.deletePost(post.id));
    this.load();
  }

  async react(post: Post): Promise<void> {
    const result = await firstValueFrom(this.api.reactToPost(post.id, '❤️'));
    this.updatePost(post.id, { reactedByMe: result.state === 'Add', reactionsCount: result.count });
  }

  showReactions(post: Post): void {
    const ref = this.modal.open(ReactionsModalComponent, { centered: true });
    (ref.componentInstance as ReactionsModalComponent).source = this.api.postReactions(post.id);
  }

  async openComments(post: Post): Promise<void> {
    const ref = this.modal.open(CommentsModalComponent, { size: 'lg', centered: true, scrollable: true });
    (ref.componentInstance as CommentsModalComponent).postId = post.id;
    await ref.result.catch(() => null);
  }

  async comment(post: Post): Promise<void> {
    const content = this.commentDrafts[post.id]?.trim();
    if (!content) return;
    await firstValueFrom(this.api.addComment(post.id, content));
    this.commentDrafts[post.id] = '';
    this.updatePost(post.id, { commentsCount: post.commentsCount + 1 });
  }

  private updatePost(id: number, changes: Partial<Post>): void {
    this.feed.update((f) => (f ? { ...f, posts: f.posts.map((p) => (p.id === id ? { ...p, ...changes } : p)) } : f));
  }
}
