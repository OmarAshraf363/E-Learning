export interface Author { id: string; fullName: string; picture: string | null }
export interface CommunityItem { id: number; name: string; description: string }
export interface UserCard { id: string; fullName: string; picture: string | null; userType: string; jopDescription: string }

export interface Post {
  id: number;
  content: string;
  attachment: string | null;
  createdAt: string;
  communityId: number | null;
  author: Author;
  keyWords: string[];
  reactionsCount: number;
  commentsCount: number;
  reactedByMe: boolean;
  isMine: boolean;
}

export interface Feed {
  communities: CommunityItem[];
  community: CommunityItem | null;
  posts: Post[];
  topInstructors: UserCard[];
  searchResults: UserCard[];
  selectedUser: (UserCard & { postsCount: number }) | null;
  currentUser: Author;
}

/** Feed filters (all in the URL, like the legacy page). */
export interface FeedFilter {
  id?: number | null;
  word?: string | null;
  maxReact?: boolean | null;
  userName?: string | null;
  userId?: string | null;
}

export interface Comment {
  id: number;
  postId: number;
  content: string;
  createdAt: string;
  author: Author;
  reactionsCount: number;
  repliesCount: number;
  reactedByMe: boolean;
}

export interface Reaction { userFullName: string | null; userPicture: string | null; type: string }
export interface ReactionToggle { state: 'Add' | 'Remove'; typeReact: string | null; count: number }

export interface PostRequest {
  content: string;
  keyWords: string[];
  communityId: number | null;
  attachment: File | null;
}
