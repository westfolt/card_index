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
  selector: 'app-user-update',
  templateUrl: './user-update.component.html',
  styleUrls: ['./user-update.component.css']
})
export class UserUpdateComponent implements OnInit {

  userEntity: userInfoModel;
  public errorMessage: string = '';
  public showError: boolean;
  public userUpdateForm: FormGroup;
  allRoles: userRoleInfoModel[];
  bsModalRef?: BsModalRef;

  constructor(private service: UserService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.getAllRoles();
    this.getUserById();
    this.userUpdateForm = new FormGroup({
      firstName: new FormControl('', [Validators.required, Validators.pattern(/^[a-zа-яїіє\.\,\-]{3,}$/i)]),
      lastName: new FormControl('', [Validators.required, Validators.pattern(/^[a-zа-яїіє\.\,\-]{4,}$/i)]),
      dateOfBirth: new FormControl('', [Validators.required]),
      city: new FormControl('', [Validators.pattern(/^[a-zа-яїіє\.\,\-]{3,}$/i)]),
      email: new FormControl('', [Validators.required, Validators.email]),
      phone: new FormControl(''),
      userRoles: new FormControl('')
    });
  }


    private getUserById = () => {
      const userId: string = this.activeRoute.snapshot.params['id'];
      const apiUri: string = `api/user/${userId}`;

      this.service.getUser(apiUri)
      .subscribe({
        next:(u: userInfoModel) => {
          this.userEntity = u;
          this.userUpdateForm.controls['firstName'].setValue(u.firstName);
          this.userUpdateForm.controls['lastName'].setValue(u.lastName);
          this.userUpdateForm.controls['dateOfBirth'].setValue(new Date(u.dateOfBirth));
          this.userUpdateForm.controls['city'].setValue(u.city);
          this.userUpdateForm.controls['email'].setValue(u.email);
          this.userUpdateForm.controls['phone'].setValue(u.phone);
          this.userUpdateForm.controls['userRoles'].setValue(u.userRoles[0]);
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.message;
          this.showError = true;
        }
      })
    }

    validateControl = (controlName: string) => {
      return this.userUpdateForm.get(controlName).invalid && this.userUpdateForm.get(controlName).touched
    }

    public hasError = (controlName: string, errorName: string)=>{
      return this.userUpdateForm.get(controlName).hasError(errorName);
    }

    updateUser = (userUpdateFormValue) => {
      this.showError = false;
      const formValues = {... userUpdateFormValue};

      const userUpdated: userInfoModel = {
        id: this.userEntity.id,
        firstName: userUpdateFormValue.title,
        lastName: userUpdateFormValue.lastName,
        dateOfBirth: userUpdateFormValue.dateOfBirth,
        city: userUpdateFormValue.city,
        email: userUpdateFormValue.email,
        phone: userUpdateFormValue.phone,
        userRoles: userUpdateFormValue.userRoles
      }

      this.service.updateUser(`api/user/${this.userEntity.id}`, userUpdated)
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
        this.bsModalRef.content.redirectOnOk.subscribe(_ => this.redirectToList());
      },
      error:(err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }})
    }
    getAllRoles = () =>{
      this.showError = false;
      this.service.getRoles(`api/user/roles`)
          .subscribe({
            next: (r: userRoleInfoModel[]) => this.allRoles = r,
            error:(err: HttpErrorResponse) => {
              this.errorMessage = err.message;
              this.showError = true;
            }
          })
        }

    redirectToList = () => {
      this.router.navigate(['/user/list']);
    }

}
