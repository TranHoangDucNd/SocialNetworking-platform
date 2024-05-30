import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutComponent } from './layout.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import {MatListModule} from '@angular/material/list';
import {MatSidenavModule} from '@angular/material/sidenav';
import { ComponentsModule } from '../components/components.module';
import { RouterModule } from '@angular/router';
import { SharedModule } from 'src/app/_modules/shared/shared.module';
import {MatTabsModule} from "@angular/material/tabs";
import {DashboardModule} from "../components/dashboard/dashboard.module";
import {CustomerroleModule} from "../components/customerrole/customerrole.module";
import {ManagepostreportModule} from "../components/managepostreport/managepostreport.module";
import {ModeratorModule} from "../components/moderator/moderator.module";
@NgModule({
  declarations: [
    LayoutComponent,
    SidebarComponent
  ],
  imports: [
    CommonModule,
    MatListModule,
    MatSidenavModule,
    ComponentsModule,
    RouterModule,
    SharedModule,
    MatTabsModule,
    DashboardModule,
    CustomerroleModule,
    ManagepostreportModule,
    ModeratorModule
  ],
  exports: [
    SidebarComponent
  ]
})
export class LayoutModule { }
