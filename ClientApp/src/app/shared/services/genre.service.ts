import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { genre } from 'src/app/_interfaces/genre';
import { EnvironmentUrlService } from './environment-url.service';

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public getGenres = (route: string) => {
    return this.http.get<genre[]>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }
  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
