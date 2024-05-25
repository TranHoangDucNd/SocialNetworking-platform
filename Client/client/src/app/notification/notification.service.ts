import {Injectable} from "@angular/core";
import {Notification, NotificationState} from "./notification.model";
import {Store} from "../nav/store";
import {notificationReducer} from "./notification.reducer";
import {environment} from "../../environments/environment";
import {HttpClient} from "@angular/common/http";


@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  #baseUrl = environment.apiUrl;
  #notificationStore: Store<NotificationState> = new Store<NotificationState>(notificationReducer, {
      unreadCount: 0,
      unreadNotifications: [],
      notifications: []
    } as NotificationState
  )

  constructor(private http: HttpClient) {
  }

  initStore(notificationState: NotificationState) {
    this.#notificationStore.dispatch({type: 'RECREATE', payload: notificationState});
  }

  addNotification(notification: Notification) {
    this.#notificationStore?.dispatch({type: 'ADD_NOTIFICATION', payload: notification});
  }

  resetStore() {
    this.#notificationStore?.dispatch({type: 'RESET'});
  }

  getStore() {
    return this.#notificationStore.value;
  }

  subscribeToStore(value: keyof NotificationState) {
    return this.#notificationStore?.select(value);
  }

  getUnreadNotifications() {
    return this.http.get<any>(this.#baseUrl + 'Notifications/get-unread');
  }

  getNewestNotifications() {
    return this.http.get<any>(this.#baseUrl + 'Notifications/get-newest');
  }

  markAsRead(notification: Notification, isRemove: boolean = false) {
    if (isRemove) {
      this.#notificationStore?.dispatch({type: 'MARK_AS_READ_AND_REMOVE', payload: notification});
    } else {
      this.#notificationStore?.dispatch({type: 'MARK_AS_READ', payload: notification});
    }
    return this.http.get(this.#baseUrl + 'Notifications/mark-as-read?notificationId=' + notification.id);
  }
  
  confirmDatingRequest(requestId: number){
    return this.http.post<any>(this.#baseUrl + 'DatingRequest/' + requestId + '/confirm-dating-request', {});
  }

  denyDatingRequest(requestId: number) {
    return this.http.post<any>(this.#baseUrl + 'DatingRequest/' + requestId + '/deny-dating-request', {});
  }

}
