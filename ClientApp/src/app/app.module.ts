import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { CollapseModule} from 'ngx-bootstrap/collapse';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AuthenticationModule } from './authentication/authentication.module';

import { AppComponent } from './app.component';
import { AuthorComponent } from './author/author.component';
import { MenuComponent } from './menu/menu.component';
import { HomeComponent } from './home/home.component';
import { NotFoundComponent } from './error-pages/not-found/not-found.component';
import { InternalServerComponent } from './error-pages/internal-server/internal-server.component';
import { ForbiddenComponent } from './error-pages/forbidden/forbidden.component';

@NgModule({
  declarations: [
    AppComponent,
    AuthorComponent,
    MenuComponent,
    HomeComponent,
    NotFoundComponent,
    InternalServerComponent,
    ForbiddenComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AuthenticationModule,
    CollapseModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
