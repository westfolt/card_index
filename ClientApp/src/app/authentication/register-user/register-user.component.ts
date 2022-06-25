import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { userRegistrationModel } from 'src/app/_interfaces/identity/userRegistrationModel';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { PasswordConfirmValidatorService } from 'src/app/shared/validators/password-confirm-validator.service';
import { response } from 'src/app/_interfaces/infrastructure/response';


@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent implements OnInit {
  public registerForm: FormGroup;
  public errorMessage: string = '';
  public showError: boolean;

  constructor(private authService: AuthenticationService, private router: Router,
    private passConfirmBalidator: PasswordConfirmValidatorService) { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      firstName: new FormControl('', [Validators.required]),
      lastName: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      phone: new FormControl(''),
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl('', [Validators.required])
    })

  }

  public validateControl = (controlName: string)=>{
    return this.registerForm.get(controlName).invalid && this.registerForm.get(controlName).touched
  }

  public hasError = (controlName: string, errorName: string)=>{
    return this.registerForm.get(controlName).hasError(errorName);
  }

  public registerUser = (registerFormValue) =>{
    this.showError = false;
    const formValues = { ... registerFormValue};


  const user: userRegistrationModel = {
    firstName: formValues.firstName,
    lastName: formValues.lastName,
    email: formValues.email,
    phone: formValues.phone,
    password: formValues.password,
    confirmPassword: formValues.confirm
  };
  this.registerForm.get('confirm').setValidators([Validators.required,
     this.passConfirmBalidator.validateConfirmPassword(this.registerForm.get('password'))]);

  this.authService.registerUser("api/authenticate/register", user)
  .subscribe({
    next: (_)=>this.router.navigate(["/authentication/login"]),
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
  }
}
