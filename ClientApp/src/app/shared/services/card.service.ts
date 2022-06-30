import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { dataShapingResponse } from 'src/app/_interfaces/infrastructure/dataShapingResponse';
import { response } from 'src/app/_interfaces/infrastructure/response';
import { textCard } from 'src/app/_interfaces/textCard';
import { EnvironmentUrlService } from './environment-url.service';

@Injectable({
  providedIn: 'root'
})
export class CardService {

  constructor(private http: HttpClient, private envUrl: EnvironmentUrlService) { }

  public getCards = (route: string) => {
    return this.http.get<dataShapingResponse<textCard>>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public getCard = (route: string) => {
    return this.http.get<textCard>(this.createCompleteRoute(route, this.envUrl.urlAddress));
  }

  public createCard = (route: string, cardEntity: textCard) => {
    return this.http.post<response>(this.createCompleteRoute(route, this.envUrl.urlAddress), cardEntity, this.generateHeaders());
  }

  public updateCard = (route: string, cardEntity: textCard) => {
    return this.http.put<response>(this.createCompleteRoute(route, this.envUrl.urlAddress), cardEntity, this.generateHeaders());
  }

  public deleteCard = (route: string) => {
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
