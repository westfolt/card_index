import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { AuthorService } from 'src/app/shared/services/author.service';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { author } from 'src/app/_interfaces/author';
import { response } from 'src/app/_interfaces/infrastructure/response';

@Component({
  selector: 'app-author-delete',
  templateUrl: './author-delete.component.html',
  styleUrls: ['./author-delete.component.css']
})
export class AuthorDeleteComponent implements OnInit {

  authorEntity: author;
  bsModalRef?: BsModalRef;
  public errorMessage: string = '';
  public showError: boolean;

  constructor(private service: AuthorService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.getAuthorById();
  }

  private getAuthorById = () => {
    const authorId: string = this.activeRoute.snapshot.params['id'];
    const apiUri: string = `api/author/${authorId}`;

    this.service.getAuthor(apiUri)
    .subscribe({
      next:(a: author) => {
        this.authorEntity = a;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
  }

  deleteAuthor = () => {
    const deleteUri: string = `api/author/${this.authorEntity.id}`;

    this.service.deleteAuthor(deleteUri)
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
    this.router.navigate(['/author/list']);
  }
}
