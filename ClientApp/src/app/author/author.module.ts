import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { AuthorRoutingModule } from './author-routing.module';
import { AuthorListComponent } from './author-list/author-list.component';
import { AuthorDetailComponent } from './author-detail/author-detail.component';
import { AuthorAddComponent } from './author-add/author-add.component';
import { AuthorUpdateComponent } from './author-update/author-update.component';
import { AuthorDeleteComponent } from './author-delete/author-delete.component';
import { MatSelectModule } from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';


@NgModule({
  declarations: [
    AuthorListComponent,
    AuthorDetailComponent,
    AuthorAddComponent,
    AuthorUpdateComponent,
    AuthorDeleteComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    AuthorRoutingModule,
    MatSelectModule,
    MatTableModule,
    MatPaginatorModule
  ]
})
export class AuthorModule { }
