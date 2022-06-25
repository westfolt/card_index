import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { userInfoModel } from 'src/app/_interfaces/identity/userInfoModel';
import { EnvironmentUrlService } from './environment-url.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public getUsers = (route: string) => {
    return this.http.get<userInfoModel[]>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }
  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
