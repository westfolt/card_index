import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { GenreRoutingModule } from './genre-routing.module';
import { GenreListComponent } from './genre-list/genre-list.component';
import { GenreDetailComponent } from './genre-detail/genre-detail.component';
import { GenreAddComponent } from './genre-add/genre-add.component';
import { GenreUpdateComponent } from './genre-update/genre-update.component';
import { GenreDeleteComponent } from './genre-delete/genre-delete.component';


@NgModule({
  declarations: [
    GenreListComponent,
    GenreDetailComponent,
    GenreAddComponent,
    GenreUpdateComponent,
    GenreDeleteComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    GenreRoutingModule
  ]
})
export class GenreModule { }
