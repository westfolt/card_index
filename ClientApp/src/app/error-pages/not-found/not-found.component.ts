import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent implements OnInit {
  errorText: string = '404 Nothing has been found!';

  constructor() { }

  ngOnInit(): void {
  }

}
