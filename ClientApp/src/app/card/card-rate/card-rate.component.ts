import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { AuthorService } from 'src/app/shared/services/author.service';
import { CardService } from 'src/app/shared/services/card.service';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { author } from 'src/app/_interfaces/author';
import { response } from 'src/app/_interfaces/infrastructure/response';
import { rateDetail } from 'src/app/_interfaces/rateDetail';
import { textCard } from 'src/app/_interfaces/textCard';

@Component({
  selector: 'app-card-rate',
  templateUrl: './card-rate.component.html',
  styleUrls: ['./card-rate.component.css']
})
export class CardRateComponent implements OnInit {

  public cardRatingForm: FormGroup;
  cardEntity: textCard;
  cardAuthors: author[];
  errorMessage: string = '';
  showError: boolean;
  cardUserRateDetails: rateDetail;
  bsModalRef?: BsModalRef;
  rateListOptions: number[] = [1,2,3,4,5];

  constructor(private service: CardService, private autService: AuthorService, private router: Router,
     private modal: BsModalService, private activeRoute: ActivatedRoute, private errorHandler: ErrorHandlerService) { }

  ngOnInit(): void {
    this.getCardDetails();
    this.cardRatingForm = new FormGroup({
      hidden: new FormControl(),
      rateGiven: new FormControl('', Validators['required'])
    });
  }

  getCardDetails = () => {
    this.showError = false;
    const id: string = this.activeRoute.snapshot.params['id'];
    const apiUrl: string = `api/card/${id}`;

    this.service.getCard(apiUrl)
    .subscribe({
      next: (tc: textCard) => {
        this.cardEntity = tc;
        this.getCardAuthors(this.cardEntity);
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
  }

  changeCardRating = (cardRatingFormValue) => {
    this.showError = false;
    const formValues = {...cardRatingFormValue};

    const rateId = (this.cardUserRateDetails !== undefined && this.cardUserRateDetails !== null)?this.cardUserRateDetails.id:0;

    const rateUpdated: rateDetail = {
      id: rateId,
      textCardId: this.cardEntity.id,
      userId: 0,
      cardName: '',
      firstName: '',
      lastName: '',
      rateValue: cardRatingFormValue.rateGiven
    }

    this.service.giveRatingToCard('api/card/rate', rateUpdated)
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
      this.bsModalRef.content.redirectOnOk.subscribe(_=>this.redirectToList());
    },
    error:(err:HttpErrorResponse) => {
      this.errorMessage = err.message;
      this.showError = true;
    }})
}

    getCardAuthors = (card: textCard) =>{
      this.showError = false;
      this.cardAuthors = new Array<author>();
      if(card.authorIds.length > 0){
        for(let i=0; i< card.authorIds.length; i++){
          this.autService.getAuthor(`api/author/${card.authorIds[i]}`)
          .subscribe({
            next: (a: author) => {
              this.cardAuthors[i] = a;
              this.getRateDetailsForCardUser(this.cardEntity.id);
            },
            error:(err: HttpErrorResponse) => {
              this.errorMessage = err.message;
              this.showError = true;
            }
          })
        }
      }
    }

    getRateDetailsForCardUser = (cardId: number) => {
      this.service.getRateDetailsForCardUser(`api/card/rate?cardId=${cardId}`)
      .subscribe({
        next: (rd: rateDetail) =>{
          this.cardUserRateDetails = rd;
          if(this.cardUserRateDetails !== undefined && this.cardUserRateDetails !== null){
            this.cardRatingForm.controls['rateGiven'].setValue(this.rateListOptions[this.cardUserRateDetails.rateValue-1]);
          }
        },
        error: (err: HttpErrorResponse) => {
          this.errorMessage = err.message;
          this.showError = true;
        }
      })
    }

redirectToList = () => {
  this.router.navigate(['/card/list']);
}

validateControl = (controlName: string) => {
  return this.cardRatingForm.get(controlName).invalid && this.cardRatingForm.get(controlName).touched
}

public hasError = (controlName: string, errorName: string)=>{
  return this.cardRatingForm.get(controlName).hasError(errorName);
}

formatDate = (release: Date): string => {
  return (release instanceof Date) ? release.toLocaleDateString(): new Date(release).toLocaleDateString();
}
}
