import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { UserService } from 'src/app/shared/services/user.service';
import { userInfoModel } from 'src/app/_interfaces/identity/userInfoModel';
import { response } from 'src/app/_interfaces/infrastructure/response';

@Component({
  selector: 'app-user-delete',
  templateUrl: './user-delete.component.html',
  styleUrls: ['./user-delete.component.css']
})
export class UserDeleteComponent implements OnInit {

  userEntity: userInfoModel;
  bsModalRef?: BsModalRef;
  public errorMessage: string = '';
  public showError: boolean;

  constructor(private service: UserService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.getUserById();
  }

  private getUserById = () => {
    const userId: string = this.activeRoute.snapshot.params['id'];
    const apiUri: string = `api/user/${userId}`;

    this.service.getUser(apiUri)
    .subscribe({
      next:(u: userInfoModel) => {
        this.userEntity = u;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
  }

  deleteUser = () => {
    const deleteUri: string = `api/user/${this.userEntity.id}`;

    this.service.deleteUser(deleteUri)
    .subscribe({
      next:(res: response) => {
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
    error: (err: HttpErrorResponse) => {
      this.errorMessage = err.message;
      this.showError = true;
    }
  })
}

  redirectToList = () => {
    this.router.navigate(['/user/list']);
  }

  formatDate = (release: Date): string => {
    return (release instanceof Date) ? release.toLocaleDateString(): new Date(release).toLocaleDateString();
  }

}
