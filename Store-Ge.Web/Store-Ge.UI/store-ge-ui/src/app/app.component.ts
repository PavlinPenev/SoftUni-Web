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

  get isUserNotLoggedIn(){
    const route = this.router.url;
    console.log(route);
    
    return route === '/login' || route === '/register' || route === '/home' || route === '/'
  }

  constructor(private router: Router){}
}
