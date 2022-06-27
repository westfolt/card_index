import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { SuccessModalComponent } from 'src/app/shared/modals/success-modal/success-modal.component';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { GenreService } from 'src/app/shared/services/genre.service';
import { genre } from 'src/app/_interfaces/genre';
import { response } from 'src/app/_interfaces/infrastructure/response';

@Component({
  selector: 'app-genre-delete',
  templateUrl: './genre-delete.component.html',
  styleUrls: ['./genre-delete.component.css']
})
export class GenreDeleteComponent implements OnInit {

  genreEntity: genre;
  bsModalRef?: BsModalRef;
  public errorMessage: string = '';
  public showError: boolean;

  constructor(private service: GenreService, private errorHandler: ErrorHandlerService,
    private router: Router, private modal: BsModalService, private activeRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.getGenreByName();
  }

  private getGenreByName = () => {
    const genreName: string = this.activeRoute.snapshot.params['name'];
    const apiUri: string = `api/genre/${genreName}`;

    this.service.getGenre(apiUri)
    .subscribe({
      next:(g: genre) => {
        this.genreEntity = g;
      },
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
  }

  deleteGenre = () => {
    const deleteUri: string = `api/genre/${this.genreEntity.id}`;

    this.service.deleteGenre(deleteUri)
    .subscribe({
      next:(res: response) => {
        const config: ModalOptions = {
          initialState: {
            modalHeaderText: 'Success Message',
            modalBodyText: `${res.message}`,
            okButtonText: 'OK'
          }
      };

      this.bsModalRef = this.modal.show(SuccessModalComponent, config);
      this.bsModalRef.content.redirectOnOk.subscribe(_ => this.redirectToList());
    },
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
