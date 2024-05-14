import {Notification, NotificationState} from "./notification.model";
import {Action} from "../nav/store";

export function notificationReducer(state: NotificationState, action: Action<any>): NotificationState {
  switch (action.type) {
    case 'ADD_NOTIFICATION':
      return {
        ...state,
        notifications: [...state.notifications, action.payload as Notification],
        unreadCount: state.unreadCount + 1,
        unreadNotifications: [...state.unreadNotifications, action.payload as Notification]
      };
    case 'MARK_AS_READ':
      return {
        ...state,
        unreadCount: state.unreadCount - 1,
        unreadNotifications: state.unreadNotifications.filter(n => n.id !== action.payload?.id),
        notifications: state.notifications.map(n => n.id === action.payload?.id ? {...n, isRead: true} : n)
      };
    case 'RESET':
      return {
        unreadCount: 0,
        unreadNotifications: [],
        notifications: []
      };
    case 'RECREATE':
      return {
        notifications: action.payload.notifications,
        unreadNotifications: action.payload.unreadNotifications,
        unreadCount: action.payload.unreadCount
      }
    default:
      return state;
  }
}