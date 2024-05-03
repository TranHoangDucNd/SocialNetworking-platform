

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

export interface UserShortDto {
  id: number;
  fullName: string;
  image: string;
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