import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorService } from 'src/app/shared/services/author.service';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { author } from 'src/app/_interfaces/author';

@Component({
  selector: 'app-author-detail',
  templateUrl: './author-detail.component.html',
  styleUrls: ['./author-detail.component.css']
})
export class AuthorDetailComponent implements OnInit {
  authorEntity: author;
  errorMessage: string = '';
  showError: boolean;

  constructor(private service: AuthorService, private router: Router,
    private activeRoute: ActivatedRoute, private errorHandler: ErrorHandlerService) { }

  ngOnInit(): void {
    this.getAuthorDetails()
  }

  getAuthorDetails = () => {
    this.showError = false;
    const id: string = this.activeRoute.snapshot.params['id'];
    const apiUrl: string = `api/author/${id}`;

    this.service.getAuthor(apiUrl)
    .subscribe({
      next: (a: author) => this.authorEntity = a,
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
}

redirectToList = () => {
  this.router.navigate(['/author/list']);
}
}
