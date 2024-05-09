import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagepostreportComponent } from './managepostreport.component';
import { RouterModule } from '@angular/router';



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
    ])
  ]
})
export class ManagepostreportModule { }
