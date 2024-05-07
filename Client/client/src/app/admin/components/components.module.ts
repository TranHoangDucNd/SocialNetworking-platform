import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CustomerroleModule } from './customerrole/customerrole.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { ModeratorModule } from './moderator/moderator.module';
import { ManagepostreportModule } from './managepostreport/managepostreport.module';
import { PostreportdetailComponent } from './postreportdetail/postreportdetail.component';



@NgModule({
  declarations: [
    PostreportdetailComponent
  ],
  imports: [
    CommonModule,
    CustomerroleModule,
    DashboardModule,
    ModeratorModule,
    ManagepostreportModule,
  ]
})
export class ComponentsModule { }
