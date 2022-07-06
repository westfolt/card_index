import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MatMenuModule } from '@angular/material/menu';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatSelectModule} from '@angular/material/select';
import { MatTableModule } from '@angular/material/table';
import { ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatInputModule } from '@angular/material/input';

import { CardRoutingModule } from './card-routing.module';
import { CardListComponent } from './card-list/card-list.component';
import { CardDeleteComponent } from './card-delete/card-delete.component';
import { CardUpdateComponent } from './card-update/card-update.component';
import { CardAddComponent } from './card-add/card-add.component';
import { CardDetailComponent } from './card-detail/card-detail.component';
import { CardRateComponent } from './card-rate/card-rate.component';


@NgModule({
  declarations: [
    CardListComponent,
    CardDeleteComponent,
    CardUpdateComponent,
    CardAddComponent,
    CardDetailComponent,
    CardRateComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardRoutingModule,
    BsDatepickerModule.forRoot(),
    MatMenuModule,
    MatFormFieldModule,
    MatSelectModule,
    MatTableModule,
    MatPaginatorModule,
    MatInputModule
  ]
})
export class CardModule { }
