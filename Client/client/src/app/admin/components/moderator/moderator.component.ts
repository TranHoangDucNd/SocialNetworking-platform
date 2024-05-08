import { Component, OnInit } from '@angular/core';
import { Check } from '@mui/icons-material';
import { ToastrService } from 'ngx-toastr';
import { LockUser, MembersLock } from 'src/app/_models/admin';
import { AdminService } from 'src/app/_service/admin.service';

@Component({
  selector: 'app-moderator',
  templateUrl: './moderator.component.html',
  styleUrls: ['./moderator.component.css']
})
export class ModeratorComponent implements OnInit {

  members: MembersLock[] = [];
  lockUser: LockUser = {
    userName: '',
    check: false
  }
  username: string = '';

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
        }
      }
    })
  }

  resetFilter(){
    this.username = '';
    this.getMembersByAdmin();
  }

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

}
