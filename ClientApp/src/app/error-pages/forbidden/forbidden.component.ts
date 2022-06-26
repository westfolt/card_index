import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'forbidden-component',
  templateUrl: './forbidden.component.html',
  styleUrls: ['./forbidden.component.css']
})
export class ForbiddenComponent implements OnInit {
  errorText: string = 'Access denied';
  errorFromRoute: string = '';
  showError: boolean = false;
  private returnUrl: string;

  constructor(private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    this.errorFromRoute = this.route.snapshot.queryParams['state'];
    if(this.errorFromRoute!=''){
      this.showError = true;
    }
  }

  public navigateToLogin = () => {
    this.router.navigate(['/authentication/login'], {queryParams: {returnUrl: this.returnUrl}});
  }
}

