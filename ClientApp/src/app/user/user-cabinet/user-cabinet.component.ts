import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { UserService } from 'src/app/shared/services/user.service';
import { userInfoModel } from 'src/app/_interfaces/identity/userInfoModel';
import { userRoleInfoModel } from 'src/app/_interfaces/identity/userRoleInfoModel';
import { response } from 'src/app/_interfaces/infrastructure/response';

@Component({
  selector: 'app-user-cabinet',
  templateUrl: './user-cabinet.component.html',
  styleUrls: ['./user-cabinet.component.css']
})
export class UserCabinetComponent implements OnInit {

  userEntity: userInfoModel;
  public errorMessage: string = '';
  public showError: boolean;
  public userCabinetForm: FormGroup;
  bsModalRef?: BsModalRef;

  constructor(private service: UserService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.getUserForCabinet();
    this.userCabinetForm = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.pattern(/^[a-zа-яїіє\.\,\-]{3,}$/i)]),
      lastName: new FormControl('', [Validators.required, Validators.pattern(/^[a-zа-яїіє\.\,\-]{4,}$/i)]),
      dateOfBirth: new FormControl('', [Validators.required]),
      city: new FormControl('', [Validators.pattern(/^[a-zа-яїіє\.\,\-]{3,}$/i)]),
      email: new FormControl('', [Validators.required, Validators.email]),
      phone: new FormControl('')
    });
  }

    private getUserForCabinet = () => {
      const apiUri: string = `api/user/cabinet`;

      this.service.getUserCabinet(apiUri)
      .subscribe({
        next:(u: userInfoModel) => {
          this.userEntity = u;
          this.userCabinetForm.controls['firstName'].setValue(u.firstName);
          this.userCabinetForm.controls['lastName'].setValue(u.lastName);
          this.userCabinetForm.controls['dateOfBirth'].setValue(new Date(u.dateOfBirth));
          this.userCabinetForm.controls['city'].setValue(u.city);
          this.userCabinetForm.controls['email'].setValue(u.email);
          this.userCabinetForm.controls['phone'].setValue(u.phone);
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.message;
          this.showError = true;
        }
      })
    }

    validateControl = (controlName: string) => {
      return this.userCabinetForm.get(controlName).invalid && this.userCabinetForm.get(controlName).touched
    }

    public hasError = (controlName: string, errorName: string)=>{
      return this.userCabinetForm.get(controlName).hasError(errorName);
    }

    updateUser = (userCabinetFormValue) => {
      this.showError = false;
      const formValues = {... userCabinetFormValue};

      const userUpdated: userInfoModel = {
        id: this.userEntity.id,
        firstName: userCabinetFormValue.firstName,
        lastName: userCabinetFormValue.lastName,
        dateOfBirth: userCabinetFormValue.dateOfBirth,
        city: userCabinetFormValue.city,
        email: userCabinetFormValue.email,
        phone: userCabinetFormValue.phone,
        userRoles: this.userEntity.userRoles
      }

      this.service.updateUser(`api/user/cabinet/modify`, userUpdated)
      .subscribe({
        next: (res: response) => {
          const config: ModalOptions = {
            initialState: {
              modalHeaderText: 'Success Message',
              modalBodyText: `${res.message}`,
              okButtonText: 'OK'
            }
        };

        this.bsModalRef = this.modal.show(SuccessModalComponent, config);
        this.bsModalRef.content.redirectOnOk.subscribe(_ => this.redirectToHome());
      },
      error:(err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }})
    }

    redirectToHome = () => {
      this.router.navigate(['/home']);
    }

}
