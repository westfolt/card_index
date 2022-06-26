import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { AuthorService } from 'src/app/shared/services/author.service';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { author } from 'src/app/_interfaces/author';
import { response } from 'src/app/_interfaces/infrastructure/response';

@Component({
  selector: 'app-author-update',
  templateUrl: './author-update.component.html',
  styleUrls: ['./author-update.component.css']
})
export class AuthorUpdateComponent implements OnInit {

  authorEntity: author;
  public errorMessage: string = '';
  public showError: boolean;
  public authorUpdateForm: FormGroup;
  bsModalRef?: BsModalRef;

  constructor(private service: AuthorService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.getAuthorById();
    this.authorUpdateForm = new FormGroup({
      firstName: new FormControl('', [Validators.required,]),
      lastName: new FormControl('', [Validators.required]),
      yearOfBirth: new FormControl('', [Validators.required, Validators.pattern(/^\d{4}$/g)])
    });
  }


    private getAuthorById = () => {
      const authorId: string = this.activeRoute.snapshot.params['id'];
      const apiUri: string = `api/author/${authorId}`;

      this.service.getAuthor(apiUri)
      .subscribe({
        next:(a: author) => {
          this.authorEntity = a;
          this.authorUpdateForm.controls['firstName'].setValue(a.firstName);
          this.authorUpdateForm.controls['lastName'].setValue(a.lastName);
          this.authorUpdateForm.controls['yearOfBirth'].setValue(a.yearOfBirth);
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.message;
          this.showError = true;
        }
      })
    }

    validateControl = (controlName: string) => {
      return this.authorUpdateForm.get(controlName).invalid && this.authorUpdateForm.get(controlName).touched
    }

    public hasError = (controlName: string, errorName: string)=>{
      return this.authorUpdateForm.get(controlName).hasError(errorName);
    }

    updateAuthor = (authorUpdateFormValue) => {
      this.showError = false;
      const formValues = {... authorUpdateFormValue};

      const authorUpdated: author = {
        id: this.authorEntity.id,
        firstName: authorUpdateFormValue.firstName,
        lastName: authorUpdateFormValue.lastName,
        yearOfBirth: authorUpdateFormValue.yearOfBirth,
        textCardIds: this.authorEntity.textCardIds
      }

      this.service.updateAuthor(`api/author/${this.authorEntity.id}`, authorUpdated)
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

    redirectToList = () => {
      this.router.navigate(['/author/list']);
    }
}
