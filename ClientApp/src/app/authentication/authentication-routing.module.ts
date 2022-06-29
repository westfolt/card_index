import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShouldLogOutGuard } from '../shared/guards/should-log-out.guard';
import { LoginComponent } from './login/login.component';
import { RegisterUserComponent } from './register-user/register-user.component';

const routes: Routes = [
  { path: 'register', component: RegisterUserComponent, canActivate: [ShouldLogOutGuard] },
  {path: 'login', component: LoginComponent, canActivate: [ShouldLogOutGuard]}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthenticationRoutingModule { }
