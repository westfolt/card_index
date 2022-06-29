import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminGuard } from '../shared/guards/admin.guard';
import { AuthGuard } from '../shared/guards/auth.guard';
import { AuthorAddComponent } from './author-add/author-add.component';
import { AuthorDeleteComponent } from './author-delete/author-delete.component';
import { AuthorDetailComponent } from './author-detail/author-detail.component';
import { AuthorListComponent } from './author-list/author-list.component';
import { AuthorUpdateComponent } from './author-update/author-update.component';

const routes: Routes = [
  { path: 'list', component: AuthorListComponent },
  { path: 'detail/:id', component: AuthorDetailComponent},
  { path: 'add', component: AuthorAddComponent, canActivate: [AuthGuard, AdminGuard]},
  {path: 'update/:id', component: AuthorUpdateComponent, canActivate: [AuthGuard, AdminGuard]},
  {path: 'delete/:id', component: AuthorDeleteComponent, canActivate: [AuthGuard, AdminGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthorRoutingModule { }
