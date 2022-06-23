import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-internal-server',
  templateUrl: './internal-server.component.html',
  styleUrls: ['./internal-server.component.css']
})
export class InternalServerComponent implements OnInit {
  errorText: string = '500 Internal server error. Contact administrator!';

  constructor() { }

  ngOnInit(): void {
  }

}
