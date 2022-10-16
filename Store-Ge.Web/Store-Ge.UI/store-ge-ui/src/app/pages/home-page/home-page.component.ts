import { Component, OnInit } from '@angular/core';
import * as textConstants from '../../../assets/text.constants';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss']
})
export class HomePageComponent implements OnInit {
  constants = textConstants;

  constructor() { }

  ngOnInit(): void {
  }

}
