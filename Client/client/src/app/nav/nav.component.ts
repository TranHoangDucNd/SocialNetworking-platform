import {Component, OnInit} from '@angular/core';
import {AccountService} from '../_service/account.service';
import {Router} from '@angular/router';
import {NotificationService} from "../notification/notification.service";

declare var $: any;

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  popoverVisible: boolean = false;
  unreadNotificationCount = 0;

  constructor(public accountService: AccountService,
              private router: Router,
              private notificationService: NotificationService) {
  }

  logout() {
    this.accountService.logout();
    this.notificationService.resetStore();
    this.router.navigateByUrl('/');
  }

  ngOnInit(): void {
    this.notificationService.subscribeToStore('unreadCount')?.subscribe((count) => {
      this.unreadNotificationCount = count as number;
    });
  }

  togglePopover(event: Event) {
    event.preventDefault(); // Prevents following the link
    this.popoverVisible = !this.popoverVisible;
  }

  closePopover() {
    this.popoverVisible = false;
  }
}
