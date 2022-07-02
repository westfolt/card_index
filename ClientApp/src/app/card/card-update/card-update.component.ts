import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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
  selector: 'app-card-update',
  templateUrl: './card-update.component.html',
  styleUrls: ['./card-update.component.css']
})
export class CardUpdateComponent implements OnInit {

  cardEntity: textCard;
  public errorMessage: string = '';
  public showError: boolean;
  public cardUpdateForm: FormGroup;
  allAuthors: author[];
  allGenres: genre[];
  bsModalRef?: BsModalRef;

  constructor(private service: CardService,private autService: AuthorService, private genService: GenreService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    console.log('start');
    this.getAllAuthors();
    this.getAllGenres();
    this.getCardById();
    this.cardUpdateForm = new FormGroup({
      title: new FormControl('', [Validators.required, Validators.pattern(/^[a-zа-яїіє\.\,\-]{6,}$/i)]),
      releaseDate: new FormControl('', [Validators.required]),
      genreName: new FormControl(''),
      authors: new FormControl('')
    });
  }


    private getCardById = () => {
      const cardId: string = this.activeRoute.snapshot.params['id'];
      const apiUri: string = `api/card/${cardId}`;

      this.service.getCard(apiUri)
      .subscribe({
        next:(c: textCard) => {
          this.cardEntity = c;
          this.cardUpdateForm.controls['title'].setValue(c.title);
          this.cardUpdateForm.controls['releaseDate'].setValue(new Date(c.releaseDate));
          this.cardUpdateForm.controls['genreName'].setValue(c.genreName);
          this.cardUpdateForm.controls['authors'].setValue(c.authorIds);
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.message;
          this.showError = true;
        }
      })
    }

    validateControl = (controlName: string) => {
      return this.cardUpdateForm.get(controlName).invalid && this.cardUpdateForm.get(controlName).touched
    }

    public hasError = (controlName: string, errorName: string)=>{
      return this.cardUpdateForm.get(controlName).hasError(errorName);
    }

    updateCard = (cardUpdateFormValue) => {
      this.showError = false;
      const formValues = {... cardUpdateFormValue};

      const cardUpdated: textCard = {
        id: this.cardEntity.id,
        title: cardUpdateFormValue.title,
        releaseDate: cardUpdateFormValue.releaseDate,
        genreName: cardUpdateFormValue.genreName,
        cardRating: this.cardEntity.cardRating,
        rateDetailIds: this.cardEntity.rateDetailIds,
        authorIds: cardUpdateFormValue.authors
      }

      this.service.updateCard(`api/card/${this.cardEntity.id}`, cardUpdated)
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
