import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CardAddComponent } from './card-add/card-add.component';
import { CardDeleteComponent } from './card-delete/card-delete.component';
import { CardDetailComponent } from './card-detail/card-detail.component';
import { CardListComponent } from './card-list/card-list.component';
import { CardUpdateComponent } from './card-update/card-update.component';

const routes: Routes = [
  {path: 'list', component: CardListComponent},
  { path: 'detail/:id', component: CardDetailComponent},
  { path: 'add', component: CardAddComponent},
  {path: 'update/:id', component: CardUpdateComponent},
  {path: 'delete/:id', component: CardDeleteComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CardRoutingModule { }
