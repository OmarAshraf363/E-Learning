import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiClient } from '../../../core/api/api-client';
import { Comment, Feed, FeedFilter, Post, PostRequest, Reaction, ReactionToggle } from './community.models';

/** Community endpoints (/api/community/...). */
@Injectable({ providedIn: 'root' })
export class CommunityApi {
  private readonly api = inject(ApiClient);

  feed = (filter: FeedFilter): Observable<Feed> => this.api.get('community/feed', { ...filter });

  createPost = (body: PostRequest): Observable<Post> => this.api.postForm('community/posts', body);
  updatePost = (id: number, body: PostRequest): Observable<Post> => this.api.putForm(`community/posts/${id}`, body);
  deletePost = (id: number): Observable<void> => this.api.delete(`community/posts/${id}`);

  comments = (postId: number): Observable<Comment[]> => this.api.get(`community/posts/${postId}/comments`);
  addComment = (postId: number, content: string): Observable<Comment> => this.api.post(`community/posts/${postId}/comments`, { content });
  replies = (commentId: number): Observable<Comment[]> => this.api.get(`community/comments/${commentId}/replies`);
  addReply = (commentId: number, content: string): Observable<Comment> => this.api.post(`community/comments/${commentId}/replies`, { content });

  reactToPost = (postId: number, typeReact: string): Observable<ReactionToggle> => this.api.post(`community/posts/${postId}/reactions`, { typeReact });
  postReactions = (postId: number): Observable<Reaction[]> => this.api.get(`community/posts/${postId}/reactions`);
  reactToComment = (commentId: number): Observable<ReactionToggle> => this.api.post(`community/comments/${commentId}/reactions`);
  commentReactions = (commentId: number): Observable<Reaction[]> => this.api.get(`community/comments/${commentId}/reactions`);
}
