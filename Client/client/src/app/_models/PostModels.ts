export interface PostResponseDto {
  id: number;
  content: string;
  images: string[];
  createdAt: string;
  updatedAt: any;
  userShort: UserShortDto;
  viewNumber: number;
  commentNumber: number;
  likeNumber: number;
  postComments: any[];
  postLikes: any[];
}

export interface CreatePostDto {
  id: number;
  content: string;
  image?: FileList;
}

export interface CommentPostDto {
  id: number;
  userId?: number;
  postId: number;
  userShort?: UserShortDto | null;
  content: string;
  createdAt: Date | string;
  updatedAt: Date | string | null;
}

export interface PostFpkDto {
  postId: number;
  userId: number;
}

export interface PostLike {
  id: number;
  postId: number;
  userId?: number;
}

export interface PostReportDto {
  id: number;
  userId: number;
  postId: number;
  description: string;
  reportDate: Date;
  checked: boolean;
  report: number;
}

export interface CountLikesAndPosts {
  likes: number
  comments: number
}

export interface UserShortDto {
  id: number;
  fullName: string;
  knownAs: string;
  image: string;
}

// export interface ICommentDto {
//   id: number;
//   content: string;
//   createAt: string;
//   descendents: ICommentDto[];
//   parentCommentId: number;
//   postId: number;
//   stats: any;
//   userId: number;
// }

export class CommentDto {
  id: number;
  content: string;
  createAt: string;
  descendants: CommentDto[];
  parentCommentId: number;
  postId: number;
  stats: any;
  userId: number;
  userShort?: UserShortDto | null | undefined;

  constructor(obj: any) {
    this.id = obj.id;
    this.content = obj.content;
    this.createAt = obj.createAt;
    this.descendants = obj.descendents;
    this.parentCommentId = obj.parentCommentId;
    this.postId = obj.postId;
    this.stats = obj.stats;
    this.userId = obj.userId;
  }
}

export interface ICreateCommentDto {
  userId: number;
  postId: number;
  parentCommentId: number;
  content: string;
  userShort?: UserShortDto;
}

export class CreateCommentDto {
  id?: number;
  userId: number;
  postId: number;
  parentCommentId: number;
  content: string;
  userShort?: UserShortDto;

  constructor(obj: any) {
    this.id = obj.id;
    this.userId = obj.userId;
    this.postId = obj.postId;
    this.parentCommentId = obj.parentCommentId;
    this.content = obj.content;
    this.userShort = obj.userShort;
  }
}

export interface IReactionRequest {
  targetId: number;
  reactionType: ReactionType;
  target: ReactionTargetType;
}

export enum ReactionType {
  like = 0,
  love = 1,
  haha = 2,
  wow = 3,
  sad = 4,
  angry = 5
}

export enum ReactionTargetType {
  post = 0,
  comment = 1
}
