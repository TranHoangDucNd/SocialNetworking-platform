import {UserShortDto} from "../_models/PostModels";

export interface NotificationState {
  unreadCount: number;
  unreadNotifications: Notification[];
  notifications: Notification[];
}

export interface Notification {
  postId: number;
  userId: number;
  id: number;
  datingRequestId: number;
  content: string;
  type: number;
  status: number;
  createdDate: string;
  userShort?: UserShortDto;
}

export enum NotificationType {
  ReactionPost = 0,
  CommentPost = 1,
  ReplyComment = 2,
  ReactionComment = 3,
  NewPost = 4,
  SentDatingRequest = 5,
  ConfirmedDatingRequest = 6,
  DeniedDatingRequest = 7,
  CancelDating = 8,
}