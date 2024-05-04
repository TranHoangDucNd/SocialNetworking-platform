import { Component } from '@angular/core';
import { AccountService } from '../_service/account.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent {
  constructor(
    public accountService: AccountService,
    private router: Router,
    private toastr: ToastrService
  ) {}
  registerMode = false;
  user: any;

  model: any = {};

  login() {
    this.accountService.login(this.model).subscribe({
      next: (_) => {
        this.router.navigateByUrl('/members');
        this.model = {};
      },

      error: (error) => this.toastr.error(error.error),
    });
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  cancelRegisterMode(event: boolean) {
    this.registerMode = event;
  }
}
