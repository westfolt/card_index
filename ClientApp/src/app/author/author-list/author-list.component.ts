import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorService } from 'src/app/shared/services/author.service';
import { author } from 'src/app/_interfaces/author';

@Component({
  selector: 'app-author-list',
  templateUrl: './author-list.component.html',
  styleUrls: ['./author-list.component.css']
})
export class AuthorListComponent implements OnInit {

  public authors: author[];

  constructor(private service: AuthorService, private router: Router) { }

  ngOnInit(): void {
    this.getAuthors();
  }

  getAuthors = () => {
    const apiAddress: string = "api/author";
    this.service.getAuthors(apiAddress)
    .subscribe({
      next: (a: author[]) => this.authors = a,
      error: (err: HttpErrorResponse) => console.log(err)
    })}

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
}
