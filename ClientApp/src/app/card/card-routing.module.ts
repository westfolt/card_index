import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminOrModeratorGuard } from '../shared/guards/admin-or-moderator.guard';
import { AuthGuard } from '../shared/guards/auth.guard';
import { CardAddComponent } from './card-add/card-add.component';
import { CardDeleteComponent } from './card-delete/card-delete.component';
import { CardDetailComponent } from './card-detail/card-detail.component';
import { CardListComponent } from './card-list/card-list.component';
import { CardUpdateComponent } from './card-update/card-update.component';

const routes: Routes = [
  {path: 'list', component: CardListComponent},
  { path: 'detail/:id', component: CardDetailComponent},
  { path: 'add', component: CardAddComponent, canActivate: [AuthGuard, AdminOrModeratorGuard]},
  {path: 'update/:id', component: CardUpdateComponent, canActivate: [AuthGuard, AdminOrModeratorGuard]},
  {path: 'delete/:id', component: CardDeleteComponent, canActivate: [AuthGuard, AdminOrModeratorGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CardRoutingModule { }
