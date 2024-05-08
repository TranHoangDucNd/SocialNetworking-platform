import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModeratorComponent } from './moderator.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    ModeratorComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: "", component: ModeratorComponent}
    ]),
    FormsModule
  ]
})
export class ModeratorModule { }
