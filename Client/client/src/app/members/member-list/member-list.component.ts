import {Component, inject, OnDestroy, OnInit} from '@angular/core';
import {BsModalService} from 'ngx-bootstrap/modal';

import {Member} from 'src/app/_models/member';
import {Pagination} from 'src/app/_models/pagination';
import {UserParams} from 'src/app/_models/userParams';
import {AccountService} from 'src/app/_service/account.service';
import {MembersService} from 'src/app/_service/members.service';
import {DatingProfileComponent} from 'src/app/dating-profile/dating-profile.component';
import {MatDialog} from "@angular/material/dialog";
import {MemberFilterComponent} from "../../modals/member-filter/member-filter.component";
import {forkJoin} from "rxjs";

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css'],
})
export class MemberListComponent implements OnInit, OnDestroy {
  members: Member[] = [];
  pagination: Pagination | undefined;
  userParams: UserParams = new UserParams();
  orderBy = 'lastActive';
  pageSize = 4;
  pageNumber = 1;

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
        this.userParams = result;
        this.loadMembers();
      }
    });
  }

  ngOnInit(): void {
    this.userParams = this.memberService.getUserParams();
    this.getOptionValues();
    this.checkUser();
    this.loadMembers();
  }

  ngOnDestroy() {
    this.memberService.memberCache.clear();
  }

  getOptionValues() {
    forkJoin([this.memberService.getProvinces(), this.memberService.getGenders()]).subscribe({
      next: (response) => {
        if (response) {
          this.memberService.provinces = response[0];
          this.memberService.genders = response[1];
        }
      }
    });
  }

  handleSortChange(event: any) {
    if (this.userParams && this.userParams.orderBy !== event.value) {
      this.userParams.orderBy = event.value;
      this.loadMembers();
    }
  }

  loadMembers() {
    if (this.userParams) {
      this.userParams.pageNumber = this.pageNumber;
      this.userParams.pageSize = this.pageSize;
      this.userParams.orderBy = this.orderBy;
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

  pageChanged(event: any) {
    this.pageNumber = event.page;
    if (this.userParams && this.userParams?.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
    }
  }
}
