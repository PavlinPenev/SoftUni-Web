import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import * as textConstants from '../assets/text.constants';
import { AccountsService } from './services/accounts.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  constants = textConstants;
  currentYear = new Date().getFullYear();
  route: string = '';

  get isUserLoggedIn() {
    this.route = this.router.url;

    return this.accountsService.isLoggedIn;
  }

  constructor(
    private router: Router,
    public accountsService: AccountsService,
    private cookieService: CookieService
  ) {}

  ngOnInit(): void {}

  logout(): void {
    this.accountsService.logout();
  }

  navigateToAccountSettings(): void {
    const userId = this.cookieService.get('uid');

    this.router.navigate(['/user', userId, 'account-settings']);
  }

  navigateToAllOrders(): void {
    const userId = this.cookieService.get('uid');
    this.router.navigate(['/user', userId, 'all-orders']);
  }
}
