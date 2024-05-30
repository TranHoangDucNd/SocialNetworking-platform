import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerroleComponent } from './customerrole.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import {MatInputModule} from "@angular/material/input";
import {MatTableModule} from "@angular/material/table";
import {MatSortModule} from "@angular/material/sort";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatIconModule} from "@angular/material/icon";
import {MatButtonModule} from "@angular/material/button";
import {TooltipModule} from "ngx-bootstrap/tooltip";
import {MatTooltipModule} from "@angular/material/tooltip";


@NgModule({
  declarations: [
    CustomerroleComponent
  ],
  exports: [
    CustomerroleComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: "", component: CustomerroleComponent}
    ]),
    FormsModule,
    MatInputModule,
    MatTableModule,
    MatSortModule,
    MatPaginatorModule,
    MatIconModule,
    MatButtonModule,
    TooltipModule,
    MatTooltipModule
  ]
})
export class CustomerroleModule { }
