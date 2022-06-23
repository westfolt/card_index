import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { EnvironmentUrlService } from './environment-url.service';
import { userRegistrationModel } from 'src/app/_interfaces/identity/userRegistrationModel';
import { userLoginModel } from 'src/app/_interfaces/identity/userLoginModel';
import { response } from 'src/app/_interfaces/infrastructure/response';

import { Subject } from 'rxjs';
import { RouteConfigLoadEnd } from '@angular/router';
//import {JwtHelperService} from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private authChangeSub = new Subject<boolean>();
  public authChanged = this.authChangeSub.asObservable();

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public registerUser = (route: string, body: userRegistrationModel)=>{
    return this.http.post<response>(this.createCompleteRoute(route, this.envUrl.urlAddress), body);
  }

  public loginUser = (route: string, body: userLoginModel) => {
    return this.http.post<response>(this.createCompleteRoute(route, this.envUrl.urlAddress), body);
  }

  public logout = (route: string) => {
    return this.http.post(this.createCompleteRoute(route, this.envUrl.urlAddress), null);
  }
  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
