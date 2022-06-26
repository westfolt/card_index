import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CardRoutingModule } from './card-routing.module';
import { CardListComponent } from './card-list/card-list.component';
import { CardDeleteComponent } from './card-delete/card-delete.component';
import { CardUpdateComponent } from './card-update/card-update.component';
import { CardAddComponent } from './card-add/card-add.component';
import { CardDetailComponent } from './card-detail/card-detail.component';


@NgModule({
  declarations: [
    CardListComponent,
    CardDeleteComponent,
    CardUpdateComponent,
    CardAddComponent,
    CardDetailComponent
  ],
  imports: [
    CommonModule,
    CardRoutingModule
  ]
})
export class CardModule { }
