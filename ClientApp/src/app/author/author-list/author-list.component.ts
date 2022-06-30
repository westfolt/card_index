import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { AuthorService } from 'src/app/shared/services/author.service';
import { author } from 'src/app/_interfaces/author';
import { dataShapingResponse } from 'src/app/_interfaces/infrastructure/dataShapingResponse';

@Component({
  selector: 'app-author-list',
  templateUrl: './author-list.component.html',
  styleUrls: ['./author-list.component.css']
})
export class AuthorListComponent implements OnInit {
  public totalNumber;
  public pageSize;
  public pageIndex;
  public dataSource = new MatTableDataSource<author>();
  public authors: author[];
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private service: AuthorService, private router: Router) { }

  ngOnInit(): void {
    this.getAuthors(2,0);
  }

  getAuthors = (pageSize, pageNumber) => {
    let params = this.getParamString(pageSize, pageNumber);
    const apiAddress: string = `api/author${params.toString()}`;
    this.service.getAuthors(apiAddress)
    .subscribe({
      next: (res: dataShapingResponse<author>) => {
        this.authors = res.data;
        this.dataSource = new MatTableDataSource<author>(this.authors);
        this.totalNumber = res.totalNumber;
        this.dataSource.paginator = this.paginator;
      },
      error: (err: HttpErrorResponse) => console.log(err)
    })}

    getNextData(pageSize, pageNumber){
      let params = this.getParamString(pageSize, pageNumber);
      const apiAddress: string = `api/author${params.toString()}`;

      this.service.getAuthors(apiAddress)
    .subscribe({
    next: (res: any) => {
      this.authors = res.data as author[];
    },
    error: (err: HttpErrorResponse) => console.log(err)
    })
  }

  pageChanged(event){
    this.getNextData(event.pageSize, event.pageIndex);
  }

  public redirectToCreatePage = () => {
    const redirectUrl: string = `/author/add`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToDetailsPage = (id) => {
    const redirectUrl: string = `/author/detail/${id}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToUpdatePage = (id) => {
    const redirectUrl: string = `/author/update/${id}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToDeletePage = (id) => {
    const redirectUrl: string = `/author/delete/${id}`;
    this.router.navigate([redirectUrl]);
  }

  private getParamString = (pageSize: number, pageNumber: number) => {
    return `?pageSize=${pageSize}&pageNumber=${pageNumber+1}`;
  }
}
