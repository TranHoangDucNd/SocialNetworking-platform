import {Component, inject, OnInit} from '@angular/core';
import { BsModalService } from 'ngx-bootstrap/modal';

import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_service/account.service';
import { MembersService } from 'src/app/_service/members.service';
import { DatingProfileComponent } from 'src/app/dating-profile/dating-profile.component';
import {MatDialog} from "@angular/material/dialog";
import {MemberFilterComponent} from "../../modals/member-filter/member-filter.component";

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit {
  members: Member[] = [];
  pagination: Pagination | undefined;
  userParams: UserParams | undefined;
  genderList = [
    { value: 'male', display: 'Males' },
    { value: 'female', display: 'Females' },
  ];

  _dialog = inject(MatDialog);

  constructor(
    private memberService: MembersService,
    private accountService: AccountService,
    private modalService: BsModalService
  ) {

  }
  checkUser() {
    this.accountService.checkDatingProfile().subscribe(
      (data: any) => {
        if (data == false) this.openDatingModal();
      },
      (error: any) => {
        console.log(error);
      }
    );
  }
  openDatingModal() {
    this.modalService.show(DatingProfileComponent, {
      class: 'modal-lg',
    });
  }

  openFilterModal() {
    const dialogRef = this._dialog.open(MemberFilterComponent, {
      width: '50%',
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadMembers();
      }
    });
  }

  ngOnInit(): void {
    this.userParams = this.memberService.getUserParams();
    this.checkUser();
    this.loadMembers();
  }

  loadMembers() {
    if (this.userParams) {
      this.memberService.setUserParams(this.userParams);
      this.memberService.getMembers(this.userParams).subscribe({
        next: (response) => {
          if (response.result && response.pagination) {
            this.members = response.result;
            this.pagination = response.pagination;
          }
        },
      });
    }
  }

  resetFiters() {
    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    if (this.userParams && this.userParams?.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
    }
  }
}
