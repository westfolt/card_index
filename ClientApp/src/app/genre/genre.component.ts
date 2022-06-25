import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { GenreService } from '../shared/services/genre.service';
import { genre } from '../_interfaces/genre';

@Component({
  selector: 'app-genre',
  templateUrl: './genre.component.html',
  styleUrls: ['./genre.component.css']
})
export class GenreComponent implements OnInit {
  public genres: genre[];

  constructor(private service: GenreService) { }

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

}
