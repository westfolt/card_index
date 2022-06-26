import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GenreService } from 'src/app/shared/services/genre.service';
import { genre } from 'src/app/_interfaces/genre';

@Component({
  selector: 'app-genre-list',
  templateUrl: './genre-list.component.html',
  styleUrls: ['./genre-list.component.css']
})
export class GenreListComponent implements OnInit {

  public genres: genre[];

  constructor(private service: GenreService, private router: Router) { }

  ngOnInit(): void {
    this.getGenres();
  }

  getGenres = () => {
    const apiAddress: string = "api/Genre";
    this.service.getGenres(apiAddress)
    .subscribe({
      next: (g: genre[]) => this.genres = g,
      error: (err: HttpErrorResponse) => console.log(err)
    })
  }

  public redirectToDetailsPage = (name) => {
    const redirectUrl: string = `/genre/detail/${name}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToUpdatePage = (id) => {
    const redirectUrl: string = `/genre/update/${id}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToDeletePage = (id) => {
    const redirectUrl: string = `/genre/delete/${id}`;
    this.router.navigate([redirectUrl]);
  }
}
