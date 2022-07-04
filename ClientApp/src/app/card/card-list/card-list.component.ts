import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { CardService } from 'src/app/shared/services/card.service';
import { dataShapingResponse } from 'src/app/_interfaces/infrastructure/dataShapingResponse';
import { textCard } from 'src/app/_interfaces/textCard';

@Component({
  selector: 'app-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.css']
})
export class CardListComponent implements OnInit {
  public totalNumber;
  public pageSize;
  public pageIndex;
  public dataSource = new MatTableDataSource<textCard>();
  public textCards: textCard[];
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private service: CardService, private router: Router) { }

  ngOnInit(): void {
    this.pageSize = 4;
    this.getCards(this.pageSize,0);
  }

  getCards = (pageSize, pageNumber) => {
    let params = this.getParamString(pageSize, pageNumber);
    const apiAddress: string = `api/Card${params.toString()}`;
    this.service.getCards(apiAddress)
    .subscribe({
    next: (res: dataShapingResponse<textCard>) => {
      this.textCards = res.data;
      this.dataSource = new MatTableDataSource<textCard>(this.textCards);
      this.totalNumber = res.totalNumber;
      this.dataSource.paginator = this.paginator;
    },
    error: (err: HttpErrorResponse) => console.log(err)
    })}

    getNextData(pageSize, pageNumber){
      let params = this.getParamString(pageSize, pageNumber);
      const apiAddress: string = `api/Card${params.toString()}`;

      this.service.getCards(apiAddress)
    .subscribe({
    next: (res: any) => {
      this.textCards = res.data as textCard[];
    },
    error: (err: HttpErrorResponse) => console.log(err)
    })
  }
    pageChanged(event){
      this.getNextData(event.pageSize, event.pageIndex);
    }

  public redirectToCreatePage = () => {
    const redirectUrl: string = `/card/add`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToDetailsPage = (id) => {
    const redirectUrl: string = `/card/detail/${id}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToRatePage = (id) => {
    const redirectUrl: string = `/card/rate/${id}`;
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

  private getParamString = (pageSize: number, pageNumber: number) => {
    return `?pageSize=${pageSize}&pageNumber=${pageNumber+1}`;
  }
}
