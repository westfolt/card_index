import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ErrorHandlerService } from 'src/app/shared/services/error-handler.service';
import { UserService } from 'src/app/shared/services/user.service';
import { userInfoModel } from 'src/app/_interfaces/identity/userInfoModel';

@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  userEntity: userInfoModel;
  errorMessage: string = '';
  showError: boolean;

  constructor(private service: UserService, private router: Router,
    private activeRoute: ActivatedRoute, private errorHandler: ErrorHandlerService) { }

  ngOnInit(): void {
    this.getUserDetails()
  }

  getUserDetails = () => {
    this.showError = false;
    const id: string = this.activeRoute.snapshot.params['id'];
    const apiUrl: string = `api/user/${id}`;

    this.service.getUser(apiUrl)
    .subscribe({
      next: (u: userInfoModel) => this.userEntity = u,
      error: (err: HttpErrorResponse) => {
        this.errorMessage = err.message;
        this.showError = true;
      }
    })
}

redirectToList = () => {
  this.router.navigate(['/user/list']);
}

formatDate = (release: Date): string => {
  return (release instanceof Date) ? release.toLocaleDateString(): new Date(release).toLocaleDateString();
}

}
