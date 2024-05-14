import {Component, inject, Input} from '@angular/core';
import {Notification} from '../notification.model';
import {Router} from "@angular/router";
import {AccountService} from "../../_service/account.service";
import {NotificationService} from "../notification.service";

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css']
})
export class NotificationListComponent {
  router = inject(Router);
  accountService = inject(AccountService);
  notificationService = inject(NotificationService);
  user = this.accountService.getCurrentUser();
  @Input() notifications: Notification[] = [];

  navigateToPost(noti: Notification) {
    console.log('Navigating to post', noti);
    if (noti.postId) {
      this.router.navigate(['/personalpage/', noti.postId]).then(
        () => {
          if (noti.status) this.notificationService.markAsRead(noti).subscribe();
        }
      )
    }
  }
}
