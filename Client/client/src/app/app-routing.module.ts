import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { authGuard } from './_guards/auth.guard';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { memberDetailedResolver } from './_resolvers/member-detailed.resolver';
import { ListsComponent } from './lists/lists.component';

const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [authGuard],
    children: [
      {path: 'members', component: MemberListComponent},
      {path: 'member/edit', component:MemberEditComponent},
      {path: 'members/:username', component: MemberDetailComponent, resolve: {member: memberDetailedResolver}},
      {path: 'lists', component: ListsComponent}
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
