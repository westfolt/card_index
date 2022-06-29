import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class AdminOrModeratorGuard implements CanActivate {

  constructor(private authService: AuthenticationService, private router: Router){}

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot){
    if(this.authService.isUserAdmin() || this.authService.isUserModerator()){
      return true;
    }

    this.router.navigate(['/forbidden'], {queryParams: {returnUrl: state.url, state: 'You need to be admin or moderator'}});
    return false;
  }

}
