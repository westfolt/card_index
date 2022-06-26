import { author } from '../../_interfaces/author';
import { EnvironmentUrlService } from './environment-url.service';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { response } from 'src/app/_interfaces/infrastructure/response';

@Injectable({
  providedIn: 'root'
})
export class AuthorService {

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public getAuthors = (route: string) => {
    return this.http.get<author[]>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public getAuthor = (route: string) => {
    return this.http.get<author>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public createAuthor = (route: string, authorEntity: author) => {
    return this.http.post<response>(this.createCompleteRoute(route, this.envUrl.urlAddress), authorEntity, this.generateHeaders());
  }

  public updateAuthor = (route: string, authorEntity: author) => {
    return this.http.put<response>(this.createCompleteRoute(route, this.envUrl.urlAddress), authorEntity, this.generateHeaders());
  }

  public deleteAuthor = (route: string) => {
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
