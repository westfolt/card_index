import { Component, OnInit } from '@angular/core';
import { AuthorService } from './../shared/services/author.service';
import { HttpErrorResponse } from '@angular/common/http';
import { Author } from './../_interfaces/author.model';

@Component({
  selector: 'app-author',
  templateUrl: './author.component.html',
  styleUrls: ['./author.component.css']
})
export class AuthorComponent implements OnInit {
  public authors: Author[];

  constructor(private service: AuthorService) { }

  ngOnInit(): void {
    this.getAuthors();
  }

  getAuthors = () => {
    const apiAddress: string = "api/Author";
    this.service.getAuthors(apiAddress)
    .subscribe({
      next: (a: Author[]) => this.authors = a,
      error: (err: HttpErrorResponse) => console.log(err)
    })}
}
