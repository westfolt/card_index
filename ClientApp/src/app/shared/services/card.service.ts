import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { textCard } from 'src/app/_interfaces/textCard';
import { EnvironmentUrlService } from './environment-url.service';

@Injectable({
  providedIn: 'root'
})
export class CardService {

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public getCards = (route: string) => {
    return this.http.get<textCard[]>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }
  private createCompleteRoute = (route: string, envAddress: string) => {
    return `${envAddress}/${route}`;
  }
}
