import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { GenreService } from 'src/app/shared/services/genre.service';
import { genre } from 'src/app/_interfaces/genre';
import { response } from 'src/app/_interfaces/infrastructure/response';

@Component({
  selector: 'app-genre-add',
  templateUrl: './genre-add.component.html',
  styleUrls: ['./genre-add.component.css']
})
export class GenreAddComponent implements OnInit {

  public errorMessage: string = '';
  public showError: boolean;
  public genreAddForm: FormGroup;
  bsModalRef?: BsModalRef;

  constructor(private service: GenreService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService) { }

  ngOnInit(): void {
    this.genreAddForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.pattern(/^[a-zа-яїіє\.\,\-]{3,}$/i)])
    });
  }

  validateControl = (controlName: string) => {
    return this.genreAddForm.get(controlName).invalid && this.genreAddForm.get(controlName).touched
  }

  public hasError = (controlName: string, errorName: string)=>{
    return this.genreAddForm.get(controlName).hasError(errorName);
  }

  createGenre = (genreAddFormValue) => {
    this.showError = false;
    const formValues = {... genreAddFormValue};

    const genreEntity: genre = {
      id: 0,
      title: formValues.title,
      textCardIds: []
    };

     this.service.createGenre('api/genre', genreEntity)
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
  this.router.navigate(['/genre/list']);
}

}
