import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/_models/user';
import { AdminService } from 'src/app/_service/admin.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { RolesModalComponent } from 'src/app/modals/roles-modal/roles-modal.component';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-customerrole',
  templateUrl: './customerrole.component.html',
  styleUrls: ['./customerrole.component.css']
})
export class CustomerroleComponent implements OnInit{
  userName: string = '';
  users: User[] = [];
  bsModalRef: BsModalRef<RolesModalComponent> = new BsModalRef<RolesModalComponent>();
  availableRoles = [
    'Admin',
    'Moderator',
    'Member',
    'ManageReport'
  ]

  constructor(private adminService: AdminService,
    private modalService: BsModalService,
    private toastr: ToastrService
  ){}
  ngOnInit(): void {
    this.findUsersWithRoles();
  }

  findUsersWithRoles(){
    return this.adminService.findUsersWithRoles(this.userName).subscribe(
     {
      next: (data) =>{
        if(data){
          this.users = data;
        }
      },
      error: (err) => {
        this.toastr.error('User name not found!');
      }
     }
    )
  }

  onSearch(): void {
    this.userName = '';
    this.findUsersWithRoles();
  }

  openRolesModal(user: User){

    const config = {
      class: 'modal-dialog-centered',
      initialState: {
        username: user.userName,
        availableRoles: this.availableRoles,
        selectedRoles: [...user.roles]
      }
    }
    this.bsModalRef = this.modalService.show(RolesModalComponent, config);
    this.bsModalRef.onHide?.subscribe({
      next: () =>{
        const selectedRoles = this.bsModalRef.content?.selectedRoles;
        if(!this.arrayEqual(selectedRoles!, user.roles)){
          this.adminService.updateUserRoles(user.userName, selectedRoles!).subscribe({
            next: roles => user.roles = roles
          })
        }
      }
    })
  }

  private arrayEqual(arr1: any[], arr2: any[]){
    return JSON.stringify(arr1.sort()) === JSON.stringify(arr2.sort());
  }




  
}
