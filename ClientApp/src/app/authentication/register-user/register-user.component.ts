import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { userRegistrationModel } from 'src/app/_interfaces/identity/userRegistrationModel';
import { AuthenticationService } from 'src/app/shared/services/authentication.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';


@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent implements OnInit {
  registerForm: FormGroup;
  errorMessage: string = '';
  showError: boolean;

  constructor(private authService: AuthenticationService) { }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
      firstName: new FormControl('', [Validators.required]),
      lasName: new FormControl('', [Validators.required]),
      email: new FormControl('', [Validators.required, Validators.email]),
      phone: new FormControl(''),
      password: new FormControl('', [Validators.required]),
      confirm: new FormControl('')
    })
  }

  public validateControl = (controlName: string)=>{
    return this.registerForm.get(controlName).invalid && this.registerForm.get(controlName).touched
  }

  public hasError = (controlName: string, errorName: string)=>{
    return this.registerForm.get(controlName).hasError(errorName);
  }

  public registerUser = (registerFormValue) =>{
    const formValues = { ... registerFormValue};


  const user: userRegistrationModel = {
    firstName: formValues.firstName,
    lastName: formValues.lasName,
    email: formValues.email,
    phone: formValues.phone,
    password: formValues.password,
    confirmPassword: formValues.confirm
  };

  this.authService.registerUser("api/Authenticate/Register", user)
  .subscribe({
    next: (_)=>console.log("Successfull registration"),
    error: (err: HttpErrorResponse) => console.log(err.error.errors)
  })
}
}
