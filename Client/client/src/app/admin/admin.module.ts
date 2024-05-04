import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutModule } from './layout/layout.module';
import { ComponentsModule } from './components/components.module';
import { SharedModule } from '../_modules/shared/shared.module';



@NgModule({
  declarations: [

  ],
  imports: [
    CommonModule,
    LayoutModule,
    ComponentsModule,
  
  ],
  exports: [
    LayoutModule,
    ComponentsModule
  ]
})
export class AdminModule { }
