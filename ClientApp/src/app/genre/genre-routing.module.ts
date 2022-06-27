import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { GenreAddComponent } from './genre-add/genre-add.component';
import { GenreDeleteComponent } from './genre-delete/genre-delete.component';
import { GenreDetailComponent } from './genre-detail/genre-detail.component';
import { GenreListComponent } from './genre-list/genre-list.component';
import { GenreUpdateComponent } from './genre-update/genre-update.component';

const routes: Routes = [
  {path: 'list', component: GenreListComponent},
  { path: 'detail/:name', component: GenreDetailComponent},
  { path: 'add', component: GenreAddComponent},
  {path: 'update/:name', component: GenreUpdateComponent},
  {path: 'delete/:name', component: GenreDeleteComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class GenreRoutingModule { }
