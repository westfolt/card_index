import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { UserService } from 'src/app/shared/services/user.service';
import { PasswordConfirmValidatorService } from 'src/app/shared/validators/password-confirm-validator.service';
import { changePasswordModel } from 'src/app/_interfaces/identity/changePasswordModel';
import { response } from 'src/app/_interfaces/infrastructure/response';

@Component({
  selector: 'app-user-cabinet-changepass',
  templateUrl: './user-cabinet-changepass.component.html',
  styleUrls: ['./user-cabinet-changepass.component.css']
})
export class UserCabinetChangepassComponent implements OnInit {

  public errorMessage: string = '';
  public showError: boolean;
  public changePasswordForm: FormGroup;
  bsModalRef?: BsModalRef;

  constructor(private service: UserService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService, private activeRoute: ActivatedRoute,
    private passConfirmValidator: PasswordConfirmValidatorService) { }

  ngOnInit(): void {
    this.changePasswordForm = new FormGroup({
      currentPassword: new FormControl('', [Validators.required]),
      newPassword: new FormControl('', [Validators.required]),
      confirmNewPassword: new FormControl('')
    });
    this.changePasswordForm.get('confirmNewPassword').setValidators([Validators.required,
      this.passConfirmValidator.validateConfirmPassword(this.changePasswordForm.get('newPassword'))]);
  }

    validateControl = (controlName: string) => {
      return this.changePasswordForm.get(controlName).invalid && this.changePasswordForm.get(controlName).touched
    }

    public hasError = (controlName: string, errorName: string)=>{
      return this.changePasswordForm.get(controlName).hasError(errorName);
    }

    changePassword = (changePasswordFormValue) => {
      this.showError = false;
      const formValues = {... changePasswordFormValue};

      const changePassRequest: changePasswordModel = {
        currentPassword: formValues.currentPassword,
        newPassword: formValues.newPassword,
        confirmNewPassword: formValues.confirmNewPassword
      }

      this.service.changeUserPassword(`api/User/cabinet/changepass`, changePassRequest)
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
    redirectToUserCabinet = () => {
      this.router.navigate(['/user/cabinet']);
    }
  }
