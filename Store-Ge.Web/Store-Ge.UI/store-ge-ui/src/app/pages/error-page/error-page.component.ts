import { Component, OnInit } from '@angular/core';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-error-page',
  templateUrl: './error-page.component.html',
  styleUrls: ['./error-page.component.scss'],
})
export class ErrorPageComponent implements OnInit {
  constants = constants;

  constructor() {}

  ngOnInit(): void {}
}
