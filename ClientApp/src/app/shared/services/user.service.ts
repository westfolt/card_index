import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { userInfoModel } from 'src/app/_interfaces/identity/userInfoModel';
import { userRoleInfoModel } from 'src/app/_interfaces/identity/userRoleInfoModel';
import { response } from 'src/app/_interfaces/infrastructure/response';
import { EnvironmentUrlService } from './environment-url.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public getUsers = (route: string) => {
    return this.http.get<userInfoModel[]>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public getUser = (route: string) => {
    return this.http.get<userInfoModel>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public getRoles = (route: string) => {
    return this.http.get<userRoleInfoModel[]>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public updateUser = (route: string, userEntity: userInfoModel) => {
    return this.http.put<response>(this.createCompleteRoute(route, this.envUrl.urlAddress), userEntity, this.generateHeaders());
  }

  public deleteUser = (route: string) => {
    return this.http.delete<response>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }

  private generateHeaders = () => {
    return {
      headers: new HttpHeaders({'Content-Type': 'application/json'})
    }
  }
}
