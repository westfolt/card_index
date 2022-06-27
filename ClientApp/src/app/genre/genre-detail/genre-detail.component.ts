import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { GenreService } from 'src/app/shared/services/genre.service';
import { genre } from 'src/app/_interfaces/genre';

@Component({
  selector: 'app-genre-detail',
  templateUrl: './genre-detail.component.html',
  styleUrls: ['./genre-detail.component.css']
})
export class GenreDetailComponent implements OnInit {
  genreEntity: genre;
  errorMessage: string = '';
  showError: boolean;

  constructor(private service: GenreService, private router: Router,
    private activeRoute: ActivatedRoute, private errorHandler: ErrorHandlerService) { }

  ngOnInit(): void {
    this.getGenreDetails()
  }

  getGenreDetails = () => {
    this.showError = false;
    const name: string = this.activeRoute.snapshot.params['name'];
    const apiUrl: string = `api/genre/${name}`;

    this.service.getGenre(apiUrl)
    .subscribe({
      next: (g: genre) => this.genreEntity = g,
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
}

redirectToList = () => {
  this.router.navigate(['/genre/list']);
}
}
