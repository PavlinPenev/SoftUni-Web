import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import * as textConstants from '../assets/text.constants';
import { AccountsService } from './services/accounts.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  constants = textConstants;
  currentYear = new Date().getFullYear();
  route: string = '';

  get isUserLoggedIn() {
    this.route = this.router.url;

    return this.accountsService.isLoggedIn;
  }

  constructor(
    private router: Router,
    private accountsService: AccountsService
  ) {}

  logout(): void {
    this.accountsService.logout();
  }
}
