import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { GenreRoutingModule } from './genre-routing.module';
import { GenreListComponent } from './genre-list/genre-list.component';
import { GenreDetailComponent } from './genre-detail/genre-detail.component';
import { GenreAddComponent } from './genre-add/genre-add.component';
import { GenreUpdateComponent } from './genre-update/genre-update.component';
import { GenreDeleteComponent } from './genre-delete/genre-delete.component';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';


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
    GenreRoutingModule,
    MatSelectModule,
    MatTableModule,
    MatPaginatorModule
  ]
})
export class GenreModule { }
