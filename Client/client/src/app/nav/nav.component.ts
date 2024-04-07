import { Component } from '@angular/core';
import { AccountService } from '../_service/account.service';
import { Route, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent {
  model: any = {};

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService){}

  login(){
    this.accountService.login(this.model).subscribe({
      next: (_) =>{
        this.router.navigateByUrl('/members');
        this.model = {};
      },

      error: (error) => this.toastr.error(error.error)
      
    })
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
