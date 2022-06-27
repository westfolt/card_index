import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { AuthorService } from 'src/app/shared/services/author.service';
import { CardService } from 'src/app/shared/services/card.service';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { author } from 'src/app/_interfaces/author';
import { response } from 'src/app/_interfaces/infrastructure/response';
import { textCard } from 'src/app/_interfaces/textCard';

@Component({
  selector: 'app-card-delete',
  templateUrl: './card-delete.component.html',
  styleUrls: ['./card-delete.component.css']
})
export class CardDeleteComponent implements OnInit {

  cardEntity: textCard;
  bsModalRef?: BsModalRef;
  public errorMessage: string = '';
  public showError: boolean;

  constructor(private service: CardService, private autService: AuthorService, private router: Router,
    private modal: BsModalService, private activeRoute: ActivatedRoute, private errorHandler: ErrorHandlerService) { }

  ngOnInit(): void {
    this.getCardById();
  }

  private getCardById = () => {
    const cardId: string = this.activeRoute.snapshot.params['id'];
    const apiUri: string = `api/card/${cardId}`;

    this.service.getCard(apiUri)
    .subscribe({
      next:(tc: textCard) => {
        this.cardEntity = tc;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
  }

  deleteCard = () => {
    const deleteUri: string = `api/card/${this.cardEntity.id}`;

    this.service.deleteCard(deleteUri)
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
    this.router.navigate(['/card/list']);
  }

  formatDate = (release: Date): string => {
    return (release instanceof Date) ? release.toLocaleDateString(): new Date(release).toLocaleDateString();
  }
}
