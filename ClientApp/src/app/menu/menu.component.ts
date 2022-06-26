import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from '../shared/services/authentication.service';

@Component({
  selector: 'app-menu',
  templateUrl: './menu.component.html',
  styleUrls: ['./menu.component.css']
})
export class MenuComponent implements OnInit {
  isCollapsed: boolean = false;
  isUserAuthenticated: boolean;
  isUserAdmin: boolean;

  constructor(private authService: AuthenticationService, private router: Router) {
    this.authService.authChanged
    .subscribe(res => {
      this.isUserAuthenticated = res;
      this.isUserAdmin = this.authService.isUserAdmin();
    })
   }

  ngOnInit(): void {
    this.authService.authChanged
    .subscribe(res => {
      this.isUserAuthenticated = res;
      this.isUserAdmin = this.authService.isUserAdmin();
    })
  }

  public logout = () => {
    this.authService.logout("api/authenticate/logout")
    .subscribe({
      next: () => {
        this.router.navigate(["/"]);
      },
      error: (err: HttpErrorResponse) => {
        console.log(err.message);
      }
    })
  }
}
