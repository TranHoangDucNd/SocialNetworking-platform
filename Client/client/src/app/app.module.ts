import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { NavComponent } from './nav/nav.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { PhotoEditorComponent } from './members/photo-editor/photo-editor.component';
import { HomeComponent } from './home/home.component';
import { SharedModule } from './_modules/shared/shared.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RegisterComponent } from './register/register.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { DatePickerComponent } from './_forms/date-picker/date-picker.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { RouteReuseStrategy } from '@angular/router';
import { CustomRoteReuseStrategy } from './_service/customRouteReuseStrategy';
import { DatingProfileComponent } from './dating-profile/dating-profile.component';
import { MatStepperModule } from '@angular/material/stepper';
import { MatButton, MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatRadioModule } from '@angular/material/radio';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatSelectModule } from '@angular/material/select';
import { DatingService } from './_service/Dating.service';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { PostComponent } from './post/post.component';
import { CreatepostComponent } from './post/createpost/createpost.component';
import { UpdatepostComponent } from './post/updatepost/updatepost.component';
import { PersonalpageComponent } from './personalpage/personalpage.component';
import { TimeagoModule } from "ngx-timeago";
import { TimeAgoPipe } from './_service/time-ago.pipe';
import {MatDialogModule} from '@angular/material/dialog';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { ChatComponent } from './post/chat/chat.component';
import {MatMenuModule} from '@angular/material/menu';
import {MatTooltipModule} from '@angular/material/tooltip';
import {OverlayModule} from '@angular/cdk/overlay';
import { AdminModule } from './admin/admin.module';
import { HasRoleDirective } from './_directives/has-role.directive';
import { RolesModalComponent } from './modals/roles-modal/roles-modal.component';
import { ReportComponent } from './report/report.component';
import { CountlikecommentComponent } from './post/countlikecomment/countlikecomment.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    MemberListComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    HomeComponent,
    RegisterComponent,
    TextInputComponent,
    DatePickerComponent,
    ListsComponent,
    MessagesComponent,
    DatingProfileComponent,
    PostComponent,
    CreatepostComponent,
    UpdatepostComponent,
    PersonalpageComponent,
    TimeAgoPipe,
    ChatComponent,
    RolesModalComponent,
    ReportComponent,
    CountlikecommentComponent
   
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true },
    { provide: RouteReuseStrategy, useClass: CustomRoteReuseStrategy },
   
  ],
  bootstrap: [AppComponent],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    SharedModule,
    ReactiveFormsModule,
    MatStepperModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatRadioModule,
    MatChipsModule,
    MatIconModule,
    MatAutocompleteModule,
    MatSelectModule,
    ModalModule.forRoot(), TimeagoModule.forRoot(),
    MatDialogModule,
    CKEditorModule,
    MatMenuModule,
    MatTooltipModule,
    OverlayModule,
    AdminModule
  ],
})
export class AppModule {}
