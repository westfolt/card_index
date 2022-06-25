import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CardService } from '../shared/services/card.service';
import { textCard } from '../_interfaces/textCard';

@Component({
  selector: 'app-card',
  templateUrl: './card.component.html',
  styleUrls: ['./card.component.css']
})
export class CardComponent implements OnInit {
  public textCards: textCard[];

  constructor(private service: CardService) { }

  ngOnInit(): void {
    this.getCards();
  }

  getCards = () => {
    const apiAddress: string = "api/Card";
    this.service.getCards(apiAddress)
    .subscribe({
      next: (c: textCard[]) => this.textCards = c,
      error: (err: HttpErrorResponse) => console.log(err)
    })
  }

  formatDate = (release: Date): string => {
    return (release instanceof Date) ? release.toLocaleDateString(): new Date(release).toLocaleDateString();
  }
}
