import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';

import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {HTTP_INTERCEPTORS, HttpClientModule} from '@angular/common/http';
import {NavComponent} from './nav/nav.component';
import {MemberListComponent} from './members/member-list/member-list.component';
import {MemberCardComponent} from './members/member-card/member-card.component';
import {MemberEditComponent} from './members/member-edit/member-edit.component';
import {PhotoEditorComponent} from './members/photo-editor/photo-editor.component';
import {HomeComponent} from './home/home.component';
import {SharedModule} from './_modules/shared/shared.module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {RegisterComponent} from './register/register.component';
import {TextInputComponent} from './_forms/text-input/text-input.component';
import {DatePickerComponent} from './_forms/date-picker/date-picker.component';
import {JwtInterceptor} from './_interceptors/jwt.interceptor';
import {LoadingInterceptor} from './_interceptors/loading.interceptor';
import {ListsComponent} from './lists/lists.component';
import {MessagesComponent} from './messages/messages.component';
import {RouteReuseStrategy} from '@angular/router';
import {CustomRoteReuseStrategy} from './_service/customRouteReuseStrategy';
import {DatingProfileComponent} from './dating-profile/dating-profile.component';
import {MatStepperModule} from '@angular/material/stepper';
import {MatButtonModule} from '@angular/material/button';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatRadioModule} from '@angular/material/radio';
import {MatChipsModule} from '@angular/material/chips';
import {MatIconModule} from '@angular/material/icon';
import {MatAutocompleteModule} from '@angular/material/autocomplete';
import {MatSelectModule} from '@angular/material/select';
import {ModalModule} from 'ngx-bootstrap/modal';
import {PostComponent} from './post/post.component';
import {CreatepostComponent} from './post/createpost/createpost.component';
import {UpdatepostComponent} from './post/updatepost/updatepost.component';
import {PersonalpageComponent} from './personalpage/personalpage.component';
import {TimeagoModule} from "ngx-timeago";
import {TimeAgoPipe} from './_service/time-ago.pipe';
import {MatDialogModule} from '@angular/material/dialog';
import {CKEditorModule} from '@ckeditor/ckeditor5-angular';
import {ChatComponent} from './post/chat/chat.component';
import {MatMenuModule} from '@angular/material/menu';
import {MatTooltipModule} from '@angular/material/tooltip';
import {OverlayModule} from '@angular/cdk/overlay';
import {AdminModule} from './admin/admin.module';
import {RolesModalComponent} from './modals/roles-modal/roles-modal.component';
import {ReportComponent} from './report/report.component';
import {CountlikecommentComponent} from './post/countlikecomment/countlikecomment.component';
import {MatButtonToggleModule} from "@angular/material/button-toggle";
import {NgOptimizedImage} from "@angular/common";
import {MatSidenavModule} from "@angular/material/sidenav";
import {MatTreeModule} from "@angular/material/tree";
import {MatListModule} from "@angular/material/list";
import {MatCardModule} from "@angular/material/card";
import {MatExpansionModule} from "@angular/material/expansion";
import {PostCommentComponent} from "./post/post-comment/post-comment.component";
import {PostCardComponent} from './post/post-card/post-card.component';
import {PostCommentBoxComponent} from './post/post-comment-box/post-comment-box.component';
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatBadgeModule} from "@angular/material/badge";
import {NotificationComponent} from './notification/notification.component';
import {CustomerroleModule} from "./admin/components/customerrole/customerrole.module";
import {DashboardModule} from "./admin/components/dashboard/dashboard.module";
import {ManagepostreportModule} from "./admin/components/managepostreport/managepostreport.module";
import {MatTabsModule} from "@angular/material/tabs";
import {ModeratorModule} from "./admin/components/moderator/moderator.module";
import { NotificationListComponent } from './notification/notification-list/notification-list.component';
import { PostDetailComponent } from './post/post-detail/post-detail.component';
import { ConfirmDialogComponent } from './messages/confirm-dialog/confirm-dialog.component';
import { ConfirmDatingRequestComponent } from './notification/confirm-dating-request/confirm-dating-request.component';
import { DatingComponent } from './dating/dating.component';

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
    CountlikecommentComponent,
    PostCommentComponent,
    PostCardComponent,
    PostCommentBoxComponent,
    NotificationComponent,
    NotificationListComponent,
    PostDetailComponent,
    ConfirmDialogComponent,
    ConfirmDatingRequestComponent,
    DatingComponent

  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true},
    {provide: RouteReuseStrategy, useClass: CustomRoteReuseStrategy},

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
    AdminModule, MatButtonToggleModule, NgOptimizedImage, MatSidenavModule, MatTreeModule, MatListModule, MatCardModule, MatExpansionModule, MatPaginatorModule, MatBadgeModule, CustomerroleModule, DashboardModule, ManagepostreportModule, MatTabsModule, ModeratorModule
  ],
})
export class AppModule {
}
