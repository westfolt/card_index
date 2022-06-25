import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../shared/services/user.service';
import { userInfoModel } from '../_interfaces/identity/userInfoModel';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  public users: userInfoModel[];

  constructor(private service: UserService) { }

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

  formatDate = (release: Date): string => {
    return (release instanceof Date) ? release.toLocaleDateString(): new Date(release).toLocaleDateString();
  }
}
