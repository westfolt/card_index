import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorService } from 'src/app/shared/services/author.service';
import { CardService } from 'src/app/shared/services/card.service';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { author } from 'src/app/_interfaces/author';
import { textCard } from 'src/app/_interfaces/textCard';

@Component({
  selector: 'app-card-detail',
  templateUrl: './card-detail.component.html',
  styleUrls: ['./card-detail.component.css']
})
export class CardDetailComponent implements OnInit {

  cardEntity: textCard;
  cardAuthors: author[];
  errorMessage: string = '';
  showError: boolean;

  constructor(private service: CardService, private autService: AuthorService, private router: Router,
    private activeRoute: ActivatedRoute, private errorHandler: ErrorHandlerService) { }

  ngOnInit(): void {
    this.getCardDetails()
    this.getCardAuthors(this.cardEntity);
  }

  getCardDetails = () => {
    this.showError = false;
    const id: string = this.activeRoute.snapshot.params['id'];
    const apiUrl: string = `api/card/${id}`;

    this.service.getCard(apiUrl)
    .subscribe({
      next: (tc: textCard) => {
        this.cardEntity = tc;
        this.getCardAuthors(tc);
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
  }

    getCardAuthors = (card: textCard) =>{
      this.showError = false;
      this.cardAuthors = new Array<author>();
      if(card.authorIds.length > 0){
        for(let i=0; i< card.authorIds.length; i++){
          this.autService.getAuthor(`api/author/${card.authorIds[i]}`)
          .subscribe({
            next: (a: author) => this.cardAuthors[i] = a,
            error:(err: HttpErrorResponse) => {
              this.errorMessage = err.message;
              this.showError = true;
            }
          })
        }
      }
    }


redirectToList = () => {
  this.router.navigate(['/card/list']);
}

formatDate = (release: Date): string => {
  return (release instanceof Date) ? release.toLocaleDateString(): new Date(release).toLocaleDateString();
}

}
