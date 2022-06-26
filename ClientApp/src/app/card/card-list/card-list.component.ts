import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CardService } from 'src/app/shared/services/card.service';
import { textCard } from 'src/app/_interfaces/textCard';

@Component({
  selector: 'app-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.css']
})
export class CardListComponent implements OnInit {
  public textCards: textCard[];

  constructor(private service: CardService, private router: Router) { }

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

  public redirectToDetailsPage = (id) => {
    const redirectUrl: string = `/card/detail/${id}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToUpdatePage = (id) => {
    const redirectUrl: string = `/card/update/${id}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToDeletePage = (id) => {
    const redirectUrl: string = `/card/delete/${id}`;
    this.router.navigate([redirectUrl]);
  }

  formatDate = (release: Date): string => {
    return (release instanceof Date) ? release.toLocaleDateString(): new Date(release).toLocaleDateString();
  }
}
