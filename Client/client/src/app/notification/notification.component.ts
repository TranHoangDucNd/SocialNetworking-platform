import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {HubConnection, HubConnectionBuilder} from "@microsoft/signalr";
import {UserShortDto} from "../_models/PostModels";
import {AccountService} from "../_service/account.service";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {PostService} from "../_service/post.service";
import {NotificationService} from "./notification.service";
import {forkJoin} from "rxjs";
import {Notification} from "./notification.model";

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent implements OnInit {
  private hubConnection!: HubConnection;
  allUserShort: UserShortDto[] = [];
  unreadNotifications: Notification[] = [];
  allNotifications: Notification[] = [];
  @Output() notiClicked = new EventEmitter<boolean>();

  constructor(public accountService: AccountService,
              private router: Router,
              private toastr: ToastrService,
              private postService: PostService,
              private notificationService: NotificationService) {
  }

  ngOnInit(): void {
    this.postService.getAllUsersShort().subscribe((res: any) => {
      this.allUserShort = res.resultObj;
      this.postService.setAllUsersShort(res);
    });
    this.initNotificationStore();
    this.createHubConnection();
    this.notificationService.subscribeToStore('unreadNotifications').subscribe((res: any) => {
      this.unreadNotifications = res;
    });
    this.notificationService.subscribeToStore('notifications').subscribe((res: any) => {
      this.allNotifications = res;
    });
  }

  initNotificationStore() {
    forkJoin([
      this.notificationService.getNewestNotifications(),
      this.notificationService.getUnreadNotifications(),
      this.postService.getAllUsersShort()
    ])
      .subscribe(([newestNotifications, unreadNotifications, allShort]) => {
        const all = allShort.resultObj;
        this.postService.setAllUsersShort(allShort);
        const newest = newestNotifications.map((notification: any) => {
          return {
            ...notification,
            userShort: all.find((user: any) => user.id === notification.userId)
          }
        });
        const unread = unreadNotifications.map((notification: any) => {
          return {
            ...notification,
            userShort: all.find((user: any) => user.id === notification.userId)
          }
        });
        this.notificationService.initStore({
          notifications: newest,
          unreadNotifications: unread,
          unreadCount: unread.length
        });
      })
  }

  createHubConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.postService.getNotificationSignalR(), {
        accessTokenFactory: () => this.accountService.getCurrentUser()?.token,
      })
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start().then(() => {
      console.log('Kết nối thành công');
    }).catch((error) => {
      console.log('Lỗi khi thiết lập kết nối', error)
    })

    this.hubConnection.on('SendNotification', (data: any) => {
      let date = new Date(data.createdDate);
      date.setHours(date.getHours() - 7);
      let updatedDate = date.toISOString();
      this.notificationService.addNotification({
        postId: data.postId,
        userId: data.userId,
        id: data.id,
        datingRequestId: data.datingRequestId,
        content: data.content,
        status: data.status,
        type: data.type,
        createdDate: updatedDate,
        userShort: this.allUserShort.find((user: any) => user.id === data.userId)
      });

      this.toastr.info(data.content, 'Thông báo mới');

    })
  }

  handleNotiClicked($event: boolean) {
    if ($event) {
      this.notiClicked.emit(true);
    }
  }
}
