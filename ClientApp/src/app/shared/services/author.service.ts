import { Author } from '../../_interfaces/author';
import { EnvironmentUrlService } from './environment-url.service';
import {HttpClient} from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public getAuthors = (route: string) => {
    return this.http.get<Author[]>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }
  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
