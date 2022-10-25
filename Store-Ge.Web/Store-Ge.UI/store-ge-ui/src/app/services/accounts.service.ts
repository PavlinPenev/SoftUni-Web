import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { catchError, throwError } from 'rxjs';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse } from '../models/login-response.model';
import { RefreshAccessTokenResponse } from '../models/refresh-access-token-response.model';
import { RegisterRequest } from '../models/register-request.model';
import {
  LOGIN_ENDPOINT,
  REFRESH_ACCESS_TOKEN_ENDPOINT,
  REGISTER_ENDPOINT,
  RESEND_CONFIRMATION_EMAIL_ENDPOINT,
} from '../shared/api-endpoints';

@Injectable({
  providedIn: 'root',
})
export class AccountsService {
  headers = new HttpHeaders().set('Content-Type', 'application/json');

  get isLoggedIn(): boolean {
    return !!this.getAccessToken() ? true : false;
  }

  constructor(
    private http: HttpClient,
    private cookieService: CookieService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {}

  getAccessToken() {
    return this.cookieService.get('access_token');
  }

  register(request: RegisterRequest) {
    return this.http
      .post<string>(REGISTER_ENDPOINT, request)
      .pipe(catchError(this.handleError))
      .subscribe((response: string) => {
        this.router.navigate(['/resend-email'], {
          queryParams: { email: response },
        });
      });
  }

  login(request: LoginRequest) {
    return this.http
      .post<LoginResponse>(LOGIN_ENDPOINT, request)
      .subscribe((response: LoginResponse) => {
        this.cookieService.set('access_token', response.accessToken);
        this.cookieService.set('refresh_token', response.refreshToken);

        this.router.navigate(['/user', response.id]);
      });
  }

  refreshAccessToken() {
    const refreshToken = this.cookieService.get('refresh_token');
    const userId = this.activatedRoute.snapshot.params['userId'];

    return this.http.post<RefreshAccessTokenResponse>(
      REFRESH_ACCESS_TOKEN_ENDPOINT,
      {
        options: {
          params: {
            refreshToken: refreshToken,
            userId: userId,
          },
        },
      }
    );
  }

  logout() {
    this.cookieService.delete('access_token');
    this.cookieService.delete('refresh_token');
    if (!this.getAccessToken()) {
      this.router.navigate(['/login']);
    }
  }

  resendConfirmationEmail(email: string) {
    return this.http.get(RESEND_CONFIRMATION_EMAIL_ENDPOINT, {
      params: {
        email: email,
      },
    });
  }

  handleError(error: HttpErrorResponse) {
    let msg = '';
    if (error.error instanceof ErrorEvent) {
      msg = error.error.message;
    } else {
      msg = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    return throwError(() => new Error(msg));
  }
}
