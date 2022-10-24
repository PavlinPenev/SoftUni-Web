import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';

@Injectable({
  providedIn: 'root'
})
export class AccountsService {

  constructor(private cookieService: CookieService) { }

  getToken() {
    return this.cookieService.get('access_token');
  }
}
