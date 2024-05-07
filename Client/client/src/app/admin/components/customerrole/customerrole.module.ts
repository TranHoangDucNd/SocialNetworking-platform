import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerroleComponent } from './customerrole.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    CustomerroleComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: "", component: CustomerroleComponent}
    ]),
    FormsModule
  ]
})
export class CustomerroleModule { }
