import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, PreloadingStrategy, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Observable } from 'rxjs';
import { ErrorModalComponent } from '../modals/error-modal/error-modal.component';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({
  providedIn: 'root'
})
export class ShouldLogOutGuard implements CanActivate {
  constructor(private authService: AuthenticationService, private router: Router, private modal: BsModalService){}

  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot){
      if(this.authService.isUserAuthenticated()){
        const config: ModalOptions = {
          initialState: {
            modalHeaderText: 'Error Message',
            modalBodyText: 'Please, logout first',
            okButtonText: 'OK'
          }
        };
        this.modal.show(ErrorModalComponent, config);
        this.router.navigate(['/']);
        return false;
    }

    return true;
  }

}
