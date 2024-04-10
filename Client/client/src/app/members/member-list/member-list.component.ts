import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { UserParams } from 'src/app/_models/userParams';
import { MembersService } from 'src/app/_service/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  members: Member[] = [];
  userParams: UserParams | undefined;
  genderList = [
    {value: 'male', display: 'Males'},
    {value: 'female', display: 'Females'}
  ];

  constructor(private memberService: MembersService){
    this.userParams = this.memberService.getUserParams();
  }
  ngOnInit(): void {
    
    this.loadMembers();
  }

  loadMembers(){
    if(this.userParams){
      this.memberService.setUserParams(this.userParams);
      this.memberService.getMembers(this.userParams).subscribe({
        next: (response) =>{
          if(response.result){
            this.members = response.result
          }
        }
      })
    }
  }

  resetFiters(){
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

}
