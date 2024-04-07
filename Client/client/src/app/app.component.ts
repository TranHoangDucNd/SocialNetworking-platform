import { Component, OnInit } from '@angular/core';
import { MembersService } from './_service/members.service';
import { Member } from './_models/member';
import { AccountService } from './_service/account.service';
import { User } from './_models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(private accountService: AccountService){}
  ngOnInit(): void {
    this.setCurrentUser();
  }
  
  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user: User = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }
  


}
