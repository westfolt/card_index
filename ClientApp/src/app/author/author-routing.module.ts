import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthorAddComponent } from './author-add/author-add.component';
import { AuthorDeleteComponent } from './author-delete/author-delete.component';
import { AuthorDetailComponent } from './author-detail/author-detail.component';
import { AuthorListComponent } from './author-list/author-list.component';
import { AuthorUpdateComponent } from './author-update/author-update.component';

const routes: Routes = [
  { path: 'list', component: AuthorListComponent },
  { path: 'detail/:id', component: AuthorDetailComponent},
  { path: 'add', component: AuthorAddComponent},
  {path: 'update/:id', component: AuthorUpdateComponent},
  {path: 'delete/:id', component: AuthorDeleteComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthorRoutingModule { }
