import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { response } from 'src/app/_interfaces/infrastructure/response';
import { userLoginModel } from 'src/app/_interfaces/identity/userLoginModel';
import { Router, ActivatedRoute } from '@angular/router';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { authResponse } from 'src/app/_interfaces/infrastructure/authResponse';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  private returnUrl: string;

  loginForm: FormGroup;
  errorMessage: string = '';
  showError: boolean;

  constructor(private authService: AuthenticationService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
      username: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.required])
    })

    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    this.errorMessage = this.route.snapshot.queryParams['state'] || '';
    if(this.errorMessage!=''){
      this.showError = true;
    }
  }

  validateControl = (controlName: string) => {
    return this.loginForm.get(controlName).invalid && this.loginForm.get(controlName).touched
  }

  hasError = (controlName: string, errorName: string) => {
    return this.loginForm.get(controlName).hasError(errorName)
  }

  loginUser = (loginFormValue) => {
    this.showError = false;
    const login = { ...loginFormValue};

    const userForLogin: userLoginModel = {
      email: login.username,
      password: login.password
    }

    this.authService.loginUser('api/authenticate/login', userForLogin)
    .subscribe({
      next: (res:authResponse) => {
        localStorage.setItem("token", res.token);
        this.authService.sendAuthStateChangeNotification(res.succeeded);
        this.router.navigate([this.returnUrl]);
      },
      error: (err: HttpErrorResponse)=>{
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
}
}
