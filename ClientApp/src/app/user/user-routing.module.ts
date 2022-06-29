import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserCabinetComponent } from './user-cabinet/user-cabinet.component';
import { UserDeleteComponent } from './user-delete/user-delete.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { UserListComponent } from './user-list/user-list.component';
import { UserUpdateComponent } from './user-update/user-update.component';

const routes: Routes = [
  {path: 'list', component: UserListComponent},
  { path: 'detail/:id', component: UserDetailComponent},
  {path: 'update/:id', component: UserUpdateComponent},
  {path: 'delete/:id', component: UserDeleteComponent},
  {path: 'cabinet', component: UserCabinetComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
