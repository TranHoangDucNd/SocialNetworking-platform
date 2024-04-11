import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrModule } from 'ngx-toastr';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TimeagoModule } from "ngx-timeago";
import { NgxSpinnerModule } from "ngx-spinner";
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FileUploadModule } from 'ng2-file-upload';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
    }),
    BsDropdownModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TabsModule.forRoot(),
    TimeagoModule.forRoot(),
    NgxSpinnerModule.forRoot({ type: 'line-scale' }),
    ButtonsModule.forRoot(),
    PaginationModule.forRoot(),
    FileUploadModule
  ],
  exports: [
    ToastrModule,
    BsDropdownModule,
    BsDatepickerModule,
    TabsModule,
    TimeagoModule,
    NgxSpinnerModule,
    ButtonsModule,
    PaginationModule,
    FileUploadModule
  ]
})
export class SharedModule { }
