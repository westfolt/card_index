import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './error-pages/not-found/not-found.component';
import { InternalServerComponent } from './error-pages/internal-server/internal-server.component';
import { ForbiddenComponent } from './error-pages/forbidden/forbidden.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'authentication', loadChildren: () => import('./authentication/authentication.module').then(m=>m.AuthenticationModule)},
  { path: '404', component: NotFoundComponent },
  { path: '500', component: InternalServerComponent},
  {path: 'author', loadChildren: () => import ('./author/author.module').then(m=>m.AuthorModule)},
  {path: 'card', loadChildren: () => import ('./card/card.module').then(m=>m.CardModule)},
  {path: 'genre', loadChildren: () => import ('./genre/genre.module').then(m=>m.GenreModule)},
  {path: 'user', loadChildren: () => import ('./user/user.module').then(m=>m.UserModule)},
  {path: 'forbidden', component: ForbiddenComponent},
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: '**', redirectTo: '/404', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
