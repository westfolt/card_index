import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { NavigationExtras, Router } from '@angular/router';
import { query } from '@angular/animations';
import { response } from 'src/app/_interfaces/infrastructure/response';
import { authResponse } from 'src/app/_interfaces/infrastructure/authResponse';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlerService {

  constructor(private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
    .pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = this.handleError(error);
        return throwError(() => new Error(errorMessage));
      })
    )
  }

  private handleError = (error: HttpErrorResponse) : string => {
    if(error.status === 404){
      return this.handle404Error(error);
    }
    else if(error.status === 400){
      return this.handle400Error(error);
    }
    else if(error.status === 401) {
      return this.handle401Error(error);
    }
    else if(error.status === 403) {
      return this.handle403Error(error);
    }
    else if(error.status === 500) {
      return this.handle500Error(error);
    }
  }
  //Forbidden
  private handle403Error = (error: HttpErrorResponse) => {
    this.router.navigate(["/forbidden"], { queryParams: { returnUrl: this.router.url }});

    return "Forbidden";
  }
  //ServerError
  private handle500Error = (error: HttpErrorResponse) => {
    this.router.navigate(["/500"]);
    return error.message;
  }
  //NotFound
  private handle404Error = (error: HttpErrorResponse): string => {
    this.router.navigate(['/404']);
    return error.message;
  }
  //Unauthorized
  private handle401Error = (error: HttpErrorResponse) => {
    if(this.router.url.startsWith('/authentication/login')) {
      let res = error.error as authResponse;
      let message = res.message ? (res.message + '<br>') : "";
      const values = Object.values(res.errors);

      values.map((m: string) => {
         message += m + '<br>';
      })

      return message.slice(0, -4);
      return message;
    }
    else {
      this.router.navigate(['/authentication/login'], { queryParams: { returnUrl: this.router.url, state: 'You need to be authenticated for this'}});
      return error.message;
    }
  }
  //BadRequest
  private handle400Error = (error: HttpErrorResponse): string => {
    let res = error.error as response;
    if(this.router.url === '/authentication/register' || this.router.url === '/authentication/login') {
      return this.compileErrors(res);
    }
    else{
      return res ? error.error : error.message;
    }
  }

  private compileErrors = (res: response): string => {
    let message = res.message ? (res.message + '<br>') : "";
      const values = Object.values(res.errors);

      values.map((m: string) => {
         message += m + '<br>';
      })

      return message.slice(0, -4);
  }
}
