import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import {Notification, NotificationType} from '../notification.model';
import {Router} from "@angular/router";
import {AccountService} from "../../_service/account.service";
import {NotificationService} from "../notification.service";
import {MatDialog} from "@angular/material/dialog";
import {
  ConfirmDatingRequestComponent,
  DatingRequestType
} from "../confirm-dating-request/confirm-dating-request.component";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css']
})
export class NotificationListComponent {
  router = inject(Router);
  accountService = inject(AccountService);
  dialog = inject(MatDialog);
  notificationService = inject(NotificationService);
  toast = inject(ToastrService);
  user = this.accountService.getCurrentUser();
  @Input() notifications: Notification[] = [];
  @Output() notiClicked = new EventEmitter<boolean>();

  navigateToPost(noti: Notification) {
    if (!noti.status) this.notificationService.markAsRead(noti).subscribe();
    this.notiClicked.emit(true);
    if (noti.postId) {
      this.router.navigate(['/post/', noti.postId]).catch(console.error);
    }
  }

  navigateToDating(noti: Notification) {
    if (!noti.status) this.notificationService.markAsRead(noti).subscribe();
    this.notiClicked.emit(true);
    // if (noti.postId) {
    //   this.router.navigate(['/post/', noti.postId]).catch(console.error);
    // }
    this.router.navigate(['/dating']).catch(console.error);
  }

  openConfirmPopup(noti: Notification) {
    const ref = this.dialog.open(ConfirmDatingRequestComponent, {
      data: {name: noti.userShort?.knownAs, type: DatingRequestType.Accept}
    });
    ref.afterClosed().subscribe(result => {
      if (result) {
        this.notificationService.confirmDatingRequest(noti.datingRequestId).subscribe({
          next: (res) => {
            this.toast.success(res.message);
          }
        });
      } else {
        this.notificationService.denyDatingRequest(noti.datingRequestId).subscribe({
          next: (res) => {
            this.toast.success(res.message);
          }
        });
      }
      this.notificationService.markAsRead(noti).subscribe();
      this.notiClicked.emit(true);
    });
  }

  handleNotiClicked(noti: Notification) {
    switch (noti.type) {
      case NotificationType.ReactionComment:
      case NotificationType.ReactionPost:
      case NotificationType.CommentPost:
      case NotificationType.ReplyComment:
      case NotificationType.NewPost:
        this.navigateToPost(noti);
        break;
      case NotificationType.SentDatingRequest:
        this.openConfirmPopup(noti);
        break;
      case NotificationType.ConfirmedDatingRequest:
      case NotificationType.CancelDating:
        this.navigateToDating(noti);
        break;
      default:
        if (!noti.status) this.notificationService.markAsRead(noti).subscribe();
        break;
    }
  }
}
