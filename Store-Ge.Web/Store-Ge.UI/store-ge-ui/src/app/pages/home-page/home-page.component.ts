import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { first } from 'rxjs';
import { AccountsService } from 'src/app/services/accounts.service';
import * as textConstants from 'src/assets/text.constants';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.scss'],
})
export class HomePageComponent implements OnInit {
  constants = textConstants;
  isUserLoggedIn!: boolean;

  constructor(
    private accountsService: AccountsService,
    private cookieService: CookieService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.accountsService.isLoggedIn
      .pipe(first())
      .subscribe((x) => (this.isUserLoggedIn = x));

    if (this.isUserLoggedIn) {
      const userId = this.cookieService.get('uid');

      this.router.navigate(['/user', userId]);
    }
  }
}
