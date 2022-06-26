import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService } from 'src/app/shared/services/user.service';
import { userInfoModel } from 'src/app/_interfaces/identity/userInfoModel';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  public users: userInfoModel[];

  constructor(private service: UserService, private router: Router) { }

  ngOnInit(): void {
    this.getUsers();
  }

  getUsers = () => {
    const apiAddress: string = "api/User";
    this.service.getUsers(apiAddress)
    .subscribe({
      next: (u: userInfoModel[]) => this.users = u,
      error: (err: HttpErrorResponse) => console.log(err)
    })
  }

  public redirectToDetailsPage = (id) => {
    const redirectUrl: string = `/user/detail/${id}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToUpdatePage = (id) => {
    const redirectUrl: string = `/user/update/${id}`;
    this.router.navigate([redirectUrl]);
  }

  public redirectToDeletePage = (id) => {
    const redirectUrl: string = `/user/delete/${id}`;
    this.router.navigate([redirectUrl]);
  }

  formatDate = (release: Date): string => {
    return (release instanceof Date) ? release.toLocaleDateString(): new Date(release).toLocaleDateString();
  }

}
