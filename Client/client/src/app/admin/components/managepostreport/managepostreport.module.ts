import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagepostreportComponent } from './managepostreport.component';
import { RouterModule } from '@angular/router';
import {MatButtonModule} from "@angular/material/button";
import {MatChipsModule} from "@angular/material/chips";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatIconModule} from "@angular/material/icon";
import {MatInputModule} from "@angular/material/input";
import {MatSortModule} from "@angular/material/sort";
import {MatTableModule} from "@angular/material/table";
import {MatTooltipModule} from "@angular/material/tooltip";



@NgModule({
  declarations: [
    ManagepostreportComponent
  ],
  exports: [
    ManagepostreportComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: "", component: ManagepostreportComponent}
    ]),
    MatButtonModule,
    MatChipsModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSortModule,
    MatTableModule,
    MatTooltipModule
  ]
})
export class ManagepostreportModule { }
