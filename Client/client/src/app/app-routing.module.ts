import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from './home/home.component';
import {authGuard} from './_guards/auth.guard';
import {MemberListComponent} from './members/member-list/member-list.component';
import {MemberEditComponent} from './members/member-edit/member-edit.component';
import {MemberDetailComponent} from './members/member-detail/member-detail.component';
import {memberDetailedResolver} from './_resolvers/member-detailed.resolver';
import {ListsComponent} from './lists/lists.component';
import {MessagesComponent} from './messages/messages.component';
import {DatingProfileComponent} from './dating-profile/dating-profile.component';
import {PersonalpageComponent} from './personalpage/personalpage.component';
import {LayoutComponent} from './admin/layout/layout.component';
import {adminGuard} from './_guards/admin.guard';
import {PostDetailComponent} from "./post/post-detail/post-detail.component";

const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {path: 'post/:id', component: PostDetailComponent},
      {path: 'personalpage/:username', component: PersonalpageComponent},
      {path: 'members', component: MemberListComponent},
      {path: 'member/edit', component: MemberEditComponent},
      {
        path: 'members/:username',
        component: MemberDetailComponent,
        resolve: {member: memberDetailedResolver},
      },
      {path: 'lists', component: ListsComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'datingprofile', component: DatingProfileComponent},

      {
        path: "admin",
        component: LayoutComponent,
        // children: [
        //   {path: "", component: DashboardComponent},
        //   {path: "customerroles", loadChildren: ()=> import("./admin/components/customerrole/customerrole.module")
        //   .then(module => module.CustomerroleModule)},
        //   {path: "managepostreport", loadChildren: ()=> import("./admin/components/managepostreport/managepostreport.module")
        //   .then(module => module.ManagepostreportModule)},
        //   {path: "moderator", loadChildren: ()=> import("./admin/components/moderator/moderator.module")
        //   .then(module => module.ModeratorModule)},
        // ],
        canActivate: [adminGuard]
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {
}
