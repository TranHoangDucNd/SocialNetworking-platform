import { Component, OnInit } from '@angular/core';
import { Check } from '@mui/icons-material';
import { ToastrService } from 'ngx-toastr';
import { LockUser, MembersLock } from 'src/app/_models/admin';
import { AdminService } from 'src/app/_service/admin.service';
import {MatTableDataSource} from "@angular/material/table";
import {User} from "../../../_models/user";

@Component({
  selector: 'app-moderator',
  templateUrl: './moderator.component.html',
  styleUrls: ['./moderator.component.css']
})
export class ModeratorComponent implements OnInit {
  displayedColumns: string[] = ['userName', 'lock', 'action'];
  members: MembersLock[] = [];
  lockUser: LockUser = {
    userName: '',
    check: false
  }
  username: string = '';
  dataSource: MatTableDataSource<MembersLock> = new MatTableDataSource<MembersLock>(this.members);

  constructor(private adminService: AdminService,
    private toastr: ToastrService
  ){

  }

  ngOnInit(): void {
    this.getMembersByAdmin();
  }

  getMembersByAdmin(){
    this.adminService.getMembersLockAdmin(this.username).subscribe({
      next: (data)=>{
        if(data){
          console.log('lock members',data)
          this.members = data as MembersLock[];
          this.dataSource = new MatTableDataSource(this.members);
        }
      }
    })
  }

  // resetFilter(){
  //   this.username = '';
  //   this.getMembersByAdmin();
  // }

  lock(username: string){
    this.lockUser.userName = username;
    this.lockUser.check = true;

    this.adminService.setLockMember(this.lockUser).subscribe({
      next: _ =>{
        this.toastr.success("Successfully locked account");
        this.getMembersByAdmin();
      }
    })
  }

  unlock(username: string){
    this.lockUser.userName = username;
    this.lockUser.check = false;
    this.adminService.setLockMember(this.lockUser).subscribe({
      next: _ =>{
        this.toastr.success("Successfully unlocked account");
        this.getMembersByAdmin();
      }
    })
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

}
