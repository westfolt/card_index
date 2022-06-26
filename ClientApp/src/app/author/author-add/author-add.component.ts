import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ModalOptions, BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AuthorService } from 'src/app/shared/services/author.service';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { author } from 'src/app/_interfaces/author';
import { response } from 'src/app/_interfaces/infrastructure/response';
import { SuccessModalComponent } from './../../shared/modals/success-modal/success-modal.component';

@Component({
  selector: 'app-author-add',
  templateUrl: './author-add.component.html',
  styleUrls: ['./author-add.component.css']
})
export class AuthorAddComponent implements OnInit {

  public errorMessage: string = '';
  public showError: boolean;
  public authorAddForm: FormGroup;
  bsModalRef?: BsModalRef;

  constructor(private service: AuthorService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService) { }

  ngOnInit(): void {
    this.authorAddForm = new FormGroup({
      firstName: new FormControl('', [Validators.required,]),
      lastName: new FormControl('', [Validators.required]),
      yearOfBirth: new FormControl('', [Validators.required, Validators.pattern(/^\d{4}$/g)])
    });
  }

  validateControl = (controlName: string) => {
    return this.authorAddForm.get(controlName).invalid && this.authorAddForm.get(controlName).touched
  }

  public hasError = (controlName: string, errorName: string)=>{
    return this.authorAddForm.get(controlName).hasError(errorName);
  }

  createAuthor = (authorAddFormValue) => {
    this.showError = false;
    const formValues = {... authorAddFormValue};

    const authorEntity: author = {
      id: 0,
      firstName: formValues.firstName,
      lastName: formValues.lastName,
      yearOfBirth: formValues.yearOfBirth as number,
      textCardIds: []
    };

     this.service.createAuthor('api/author', authorEntity)
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
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })

  }

redirectToList = () => {
  this.router.navigate(['/author/list']);
}
}
