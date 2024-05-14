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
  content: string;
  status: number;
  createdDate: string;
  userShort?: UserShortDto;
}
