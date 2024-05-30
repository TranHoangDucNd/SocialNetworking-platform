import {ChangeDetectorRef, Component, OnInit, ViewChild} from '@angular/core';
import { MembersService } from './_service/members.service';
import { Member } from './_models/member';
import { AccountService } from './_service/account.service';
import { User } from './_models/user';
import {MatSidenav} from "@angular/material/sidenav";
import { PresenceService } from './_service/presence.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  @ViewChild('sidenav') sidenav: MatSidenav | undefined;
  isExpanded = true;
  showSubmenu: boolean = false;
  isShowing = false;
  showSubSubMenu: boolean = false;
  user: User | null = null;
  unreadMessageCount: number = 0;

  constructor(public accountService: AccountService, private cdr: ChangeDetectorRef,
    public presenceService: PresenceService){}
    ngOnInit(): void {
    this.setCurrentUser();


    this.presenceService.unreadMessageCount$.subscribe(count => {
      console.log('Unread message count updated:', count); // Kiểm tra cập nhật
      this.unreadMessageCount = count;
      this.cdr.detectChanges();
    });
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user: User = JSON.parse(userString);
    this.user = user;
    this.accountService.setCurrentUser(user);
  }

  mouseenter() {
    if (!this.isExpanded) {
      this.isShowing = true;
    }
  }

  clearUnreadMessages() {
    this.presenceService.clearUnreadMessages();
  }


  mouseleave() {
    if (!this.isExpanded) {
      this.isShowing = false;
    }
  }

}
