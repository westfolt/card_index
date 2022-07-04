import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { GenreService } from 'src/app/shared/services/genre.service';
import { genre } from 'src/app/_interfaces/genre';
import { dataShapingResponse } from 'src/app/_interfaces/infrastructure/dataShapingResponse';

@Component({
  selector: 'app-genre-list',
  templateUrl: './genre-list.component.html',
  styleUrls: ['./genre-list.component.css']
})
export class GenreListComponent implements OnInit {
  public totalNumber;
  public pageSize;
  public pageIndex;
  public dataSource = new MatTableDataSource<genre>();
  public genres: genre[];
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private service: GenreService, private router: Router) { }

  ngOnInit(): void {
    this.pageSize = 4;
    this.getGenres(this.pageSize,0);
  }

  getGenres = (pageSize, pageNumber) => {
    let params = this.getParamString(pageSize, pageNumber);
    const apiAddress: string = `api/genre${params.toString()}`;
    this.service.getGenres(apiAddress)
    .subscribe({
      next: (res: dataShapingResponse<genre>) => {
        this.genres = res.data;
        this.dataSource = new MatTableDataSource<genre>(this.genres);
        this.totalNumber = res.totalNumber;
        this.dataSource.paginator = this.paginator;
      },
      error: (err: HttpErrorResponse) => console.log(err)
    })}

    getNextData(pageSize, pageNumber){
      let params = this.getParamString(pageSize, pageNumber);
      const apiAddress: string = `api/genre${params.toString()}`;

      this.service.getGenres(apiAddress)
    .subscribe({
    next: (res: any) => {
      this.genres = res.data as genre[];
    },
    error: (err: HttpErrorResponse) => console.log(err)
    })
  }

  pageChanged(event){
    this.getNextData(event.pageSize, event.pageIndex);
  }

  public redirectToCreatePage = () => {
    const redirectUrl: string = `/genre/add`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToDetailsPage = (name) => {
    const redirectUrl: string = `/genre/detail/${name}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToUpdatePage = (name) => {
    const redirectUrl: string = `/genre/update/${name}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToDeletePage = (name) => {
    const redirectUrl: string = `/genre/delete/${name}`;
    this.router.navigate([redirectUrl]);
  }

  private getParamString = (pageSize: number, pageNumber: number) => {
    return `?pageSize=${pageSize}&pageNumber=${pageNumber+1}`;
  }
}
