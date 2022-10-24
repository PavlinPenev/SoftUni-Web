import { Component } from '@angular/core';
import { Router } from '@angular/router';
import * as textConstants from '../assets/text.constants';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constants = textConstants;
  currentYear = new Date().getFullYear();
  route: string = '';

  get isUserNotLoggedIn(){
    this.route = this.router.url;
    
    return this.route === '/login' || this.route === '/register' || this.route === '/home' || this.route === '/'
  }

  constructor(private router: Router){}
}
