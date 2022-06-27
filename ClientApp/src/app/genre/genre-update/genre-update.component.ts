import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { GenreService } from 'src/app/shared/services/genre.service';
import { genre } from 'src/app/_interfaces/genre';
import { response } from 'src/app/_interfaces/infrastructure/response';

@Component({
  selector: 'app-genre-update',
  templateUrl: './genre-update.component.html',
  styleUrls: ['./genre-update.component.css']
})
export class GenreUpdateComponent implements OnInit {
  genreEntity: genre;
  public errorMessage: string = '';
  public showError: boolean;
  public genreUpdateForm: FormGroup;
  bsModalRef?: BsModalRef;

  constructor(private service: GenreService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.getGenreById();
    this.genreUpdateForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.pattern(/^[a-zа-яїіє\.\,\-]{3,}$/i)])
    });
  }


    private getGenreById = () => {
      const genreName: string = this.activeRoute.snapshot.params['name'];
      const apiUri: string = `api/genre/${genreName}`;

      this.service.getGenre(apiUri)
      .subscribe({
        next:(g: genre) => {
          this.genreEntity = g;
          this.genreUpdateForm.controls['title'].setValue(g.title);
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.message;
          this.showError = true;
        }
      })
    }

    validateControl = (controlName: string) => {
      return this.genreUpdateForm.get(controlName).invalid && this.genreUpdateForm.get(controlName).touched
    }

    public hasError = (controlName: string, errorName: string)=>{
      return this.genreUpdateForm.get(controlName).hasError(errorName);
    }

    updateGenre = (genreUpdateFormValue) => {
      this.showError = false;
      const formValues = {... genreUpdateFormValue};

      const genreUpdated: genre = {
        id: this.genreEntity.id,
        title: genreUpdateFormValue.title,
        textCardIds: this.genreEntity.textCardIds
      }

      this.service.updateGenre(`api/genre/${this.genreEntity.id}`, genreUpdated)
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
      this.router.navigate(['/genre/list']);
    }

}
