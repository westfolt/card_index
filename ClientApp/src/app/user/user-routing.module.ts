import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from '../shared/guards/admin.guard';
import { AuthGuard } from '../shared/guards/auth.guard';
import { UserCabinetComponent } from './user-cabinet/user-cabinet.component';
import { UserDeleteComponent } from './user-delete/user-delete.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserUpdateComponent } from './user-update/user-update.component';

const routes: Routes = [
  {path: 'list', component: UserListComponent, canActivate: [AuthGuard, AdminGuard]},
  { path: 'detail/:id', component: UserDetailComponent, canActivate: [AuthGuard, AdminGuard]},
  {path: 'update/:id', component: UserUpdateComponent, canActivate: [AuthGuard, AdminGuard]},
  {path: 'delete/:id', component: UserDeleteComponent, canActivate: [AuthGuard, AdminGuard]},
  {path: 'cabinet', component: UserCabinetComponent, canActivate: [AuthGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
