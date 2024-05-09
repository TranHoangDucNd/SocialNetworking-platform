import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModeratorComponent } from './moderator.component';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import {MatButtonModule} from "@angular/material/button";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatIconModule} from "@angular/material/icon";
import {MatInputModule} from "@angular/material/input";
import {MatSortModule} from "@angular/material/sort";
import {MatTableModule} from "@angular/material/table";
import {MatTooltipModule} from "@angular/material/tooltip";
import {MatChipsModule} from "@angular/material/chips";



@NgModule({
  declarations: [
    ModeratorComponent
  ],
  exports: [
    ModeratorComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: "", component: ModeratorComponent}
    ]),
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatSortModule,
    MatTableModule,
    MatTooltipModule,
    MatChipsModule
  ]
})
export class ModeratorModule { }
