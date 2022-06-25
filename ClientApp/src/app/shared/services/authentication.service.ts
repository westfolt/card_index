import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { EnvironmentUrlService } from './environment-url.service';
import { userRegistrationModel } from 'src/app/_interfaces/identity/userRegistrationModel';
import { userLoginModel } from 'src/app/_interfaces/identity/userLoginModel';
import { response } from 'src/app/_interfaces/infrastructure/response';

import { Subject } from 'rxjs';
import { authResponse } from 'src/app/_interfaces/infrastructure/authResponse';
import {JwtHelperService} from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  private authChangeSub = new Subject<boolean>();
  public authChanged = this.authChangeSub.asObservable();

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService,
    private jwtHelper: JwtHelperService) { }

  public registerUser = (route: string, body: userRegistrationModel)=>{
    return this.http.post<response>(this.createCompleteRoute(route, this.envUrl.urlAddress), body);
  }

  public loginUser = (route: string, body: userLoginModel) => {
    return this.http.post<authResponse>(this.createCompleteRoute(route, this.envUrl.urlAddress), body);
  }

  public logout = (route: string) => {
    console.log(this.http.post<void>(this.createCompleteRoute(route, this.envUrl.urlAddress), null));
    localStorage.removeItem("token");
    this.sendAuthStateChangeNotification(false);
  }

  public isUserAuthenticated = (): boolean => {
    const token = localStorage.getItem("token");
    return token && !this.jwtHelper.isTokenExpired(token);
  }

  public isUserAdmin = (): boolean => {
    const token = localStorage.getItem("token");
    const decodedToken = this.jwtHelper.decodeToken(token);
    const role = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']

    return role === 'Administrator';
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }

  public sendAuthStateChangeNotification = (isAuthenticated: boolean) => {
    this.authChangeSub.next(isAuthenticated);
  }
}
