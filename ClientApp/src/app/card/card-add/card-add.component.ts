import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { AuthorService } from 'src/app/shared/services/author.service';
import { CardService } from 'src/app/shared/services/card.service';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { GenreService } from 'src/app/shared/services/genre.service';
import { author } from 'src/app/_interfaces/author';
import { genre } from 'src/app/_interfaces/genre';
import { response } from 'src/app/_interfaces/infrastructure/response';
import { textCard } from 'src/app/_interfaces/textCard';

@Component({
  selector: 'app-card-add',
  templateUrl: './card-add.component.html',
  styleUrls: ['./card-add.component.css']
})
export class CardAddComponent implements OnInit {

  public errorMessage: string = '';
  public showError: boolean;
  public cardAddForm: FormGroup;
  allAuthors: author[];
  allGenres: genre[];
  bsModalRef?: BsModalRef;

  constructor(private service: CardService,private autService: AuthorService, private genService: GenreService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService) { }

  ngOnInit(): void {
    this.getAllAuthors();
    this.getAllGenres();
    this.cardAddForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.pattern(/^[a-zа-яїіє\.\,\-]{6,}$/i)]),
      releaseDate: new FormControl('', [Validators.required]),
      genreName: new FormControl(''),
      authors: new FormControl('')
    });
  }

  validateControl = (controlName: string) => {
    return this.cardAddForm.get(controlName).invalid && this.cardAddForm.get(controlName).touched
  }

  public hasError = (controlName: string, errorName: string)=>{
    return this.cardAddForm.get(controlName).hasError(errorName);
  }

  createCard = (cardAddFormValue) => {
    this.showError = false;
    const formValues = {... cardAddFormValue};

    const cardEntity: textCard = {
      id: 0,
      title: formValues.title,
      releaseDate: formValues.releaseDate,
      genreName: formValues.genreName,
      cardRating: 0,
      rateDetailIds: [],
      authorIds: formValues.authors
    };

     this.service.createCard('api/card', cardEntity)
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

  getAllAuthors = () =>{
    this.showError = false;
    this.autService.getAllAuthors(`api/author/all`)
        .subscribe({
          next: (a: author[]) => this.allAuthors = a,
          error:(err: HttpErrorResponse) => {
            this.errorMessage = err.message;
            this.showError = true;
          }
        })
      }

      getAllGenres = () => {
        this.showError = false;
        this.genService.getAllGenres(`api/genre/all`)
            .subscribe({
              next: (g: genre[]) => this.allGenres = g,
              error:(err: HttpErrorResponse) => {
                this.errorMessage = err.message;
                this.showError = true;
              }
            })
          }

redirectToList = () => {
  this.router.navigate(['/card/list']);
}
}
