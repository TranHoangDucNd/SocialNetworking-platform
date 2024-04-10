import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_service/members.service';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit{

  member: Member = {} as Member;

  constructor(private route: ActivatedRoute){}
  ngOnInit(): void {
    this.route.data.subscribe({
      next: data=>{
        this.member = data['member']
      }
    })
  }

}
