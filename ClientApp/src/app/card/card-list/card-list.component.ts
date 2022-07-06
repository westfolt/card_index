import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { AuthorService } from 'src/app/shared/services/author.service';
import { CardService } from 'src/app/shared/services/card.service';
import { GenreService } from 'src/app/shared/services/genre.service';
import { author } from 'src/app/_interfaces/author';
import { genre } from 'src/app/_interfaces/genre';
import { cardFilter } from 'src/app/_interfaces/infrastructure/cardFilter';
import { dataShapingResponse } from 'src/app/_interfaces/infrastructure/dataShapingResponse';
import { textCard } from 'src/app/_interfaces/textCard';


@Component({
  selector: 'app-card-list',
  templateUrl: './card-list.component.html',
  styleUrls: ['./card-list.component.css']
})
export class CardListComponent implements OnInit {

  public cardFilterForm: FormGroup;
  public errorMessage: string = '';
  public showError: boolean;
  allAuthors: author[];
  allGenres: genre[];
  public filter: cardFilter;
  public totalNumber;
  public pageSize;
  public pageIndex;
  public dataSource = new MatTableDataSource<textCard>();
  public textCards: textCard[];
  rateListOptions: number[] = [5,4,3,2,1];
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private service: CardService, private autService: AuthorService, private genService: GenreService, private router: Router) { }

  ngOnInit(): void {
    this.pageSize = 4;
    this.getCards(this.pageSize,0, null);
    this.getAllAuthors();
    this.getAllGenres();
    this.cardFilterForm = new FormGroup({
      nameFilter: new FormControl(''),
      genreFilter: new FormControl(''),
      authorFilter: new FormControl(''),
      rateFilter: new FormControl('')
    });
  }

  getCards = (pageSize, pageNumber, filter) => {
    let params = this.getParamString(pageSize, pageNumber, filter);
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

    getNextData(pageSize, pageNumber, filter){
      let params = this.getParamString(pageSize, pageNumber, filter);
      const apiAddress: string = `api/Card${params.toString()}`;

      this.service.getCards(apiAddress)
    .subscribe({
    next: (res: any) => {
      this.textCards = res.data as textCard[];
      this.totalNumber = res.totalNumber;
    },
    error: (err: HttpErrorResponse) => console.log(err)
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

    pageChanged(event){
      this.pageSize = event.pageSize;
      this.pageIndex = event.pageIndex;
      this.getNextData(event.pageSize, event.pageIndex, this.filter);
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

  public filterCardResults = (cardFilterFormValue) => {
    this.showError = false;

      this.filter = {
        cardNameFilter: cardFilterFormValue.nameFilter,
        authorId: cardFilterFormValue.authorFilter,
        genreId: cardFilterFormValue.genreFilter,
        rating: cardFilterFormValue.rateFilter
      }

      this.getCards(this.pageSize, 0, this.filter);
    }

    formReset = () => {
      this.cardFilterForm.reset();
      this.filter = null;
      this.getNextData(this.pageSize, 0, null);
    }

  private getParamString = (pageSize: number, pageNumber: number, filter: cardFilter) => {
    if(filter === null || filter === undefined){
      return `?pageSize=${pageSize}&pageNumber=${pageNumber+1}`;
    }
    else{
      const authorPlaceholder = filter.authorId!==null && filter.authorId !== ''? `&authorId=${filter.authorId}`:'';
      const ratePlaceholder = filter.rating!==null && filter.rating !== ''? `&rating=${filter.rating}`:'';
      const genrePlaceholder = filter.genreId!==null && filter.genreId !== ''? `&genreId=${filter.genreId}`:'';
      const namePlaceholder = filter.cardNameFilter!==null && filter.cardNameFilter !== ''? `&cardName=${filter.cardNameFilter}`:'';
      return `?pageSize=${pageSize}&pageNumber=${pageNumber+1}${ratePlaceholder}${authorPlaceholder}${genrePlaceholder}${namePlaceholder}`;
    }
  }
}
