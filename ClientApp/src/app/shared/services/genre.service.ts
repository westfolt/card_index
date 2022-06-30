import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { genre } from 'src/app/_interfaces/genre';
import { dataShapingResponse } from 'src/app/_interfaces/infrastructure/dataShapingResponse';
import { response } from 'src/app/_interfaces/infrastructure/response';
import { EnvironmentUrlService } from './environment-url.service';

@Injectable({
  providedIn: 'root'
})
export class GenreService {

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public getAllGenres = (route: string) => {
    return this.http.get<genre[]>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public getGenres = (route: string) => {
    return this.http.get<dataShapingResponse<genre>>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public getGenre = (route: string) => {
    return this.http.get<genre>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public createGenre = (route: string, genreEntity: genre) => {
    return this.http.post<response>(this.createCompleteRoute(route, this.envUrl.urlAddress), genreEntity, this.generateHeaders());
  }

  public updateGenre = (route: string, genreEntity: genre) => {
    return this.http.put<response>(this.createCompleteRoute(route, this.envUrl.urlAddress), genreEntity, this.generateHeaders());
  }

  public deleteGenre = (route: string) => {
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
