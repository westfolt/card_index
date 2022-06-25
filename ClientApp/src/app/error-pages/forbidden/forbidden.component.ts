import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'forbidden-component',
  templateUrl: './forbidden.component.html',
  styleUrls: ['./forbidden.component.css']
})
export class ForbiddenComponent implements OnInit {
  errorText: string = 'Access denied';

  constructor() { }

  ngOnInit(): void {
  }

}

