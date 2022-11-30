import {
  HttpClient,
  HttpErrorResponse,
  HttpHeaders,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { catchError, filter, first, Observable, of, throwError } from 'rxjs';
import { ConfirmEmailRequest } from '../models/confirm-email.model';
import { LoginRequest } from '../models/login-request.model';
import { LoginResponse } from '../models/login-response.model';
import { RefreshAccessTokenResponse } from '../models/refresh-access-token-response.model';
import { RegisterRequest } from '../models/register-request.model';
import { ForgotPasswordRequest } from '../models/forgot-password.model';
import {
  ADD_CASHIER_ENDPOINT,
  CONFIRM_EMAIL_ENDPOINT,
  FORGOT_PASSWORD_ENDPOINT,
  GET_USER_ENDPOINT,
  LOGIN_ENDPOINT,
  REFRESH_ACCESS_TOKEN_ENDPOINT,
  REGISTER_ENDPOINT,
  RESEND_CONFIRMATION_EMAIL_ENDPOINT,
  RESET_PASSWORD_ENDPOINT,
  UPDATE_USER_ENDPOINT,
} from '../shared/api-endpoints';
import { ResetPasswordRequest } from '../models/reset-password.model';
import { User } from '../models/user.model';
import { environment } from 'src/environments/environment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { RegisterCashierRequest } from '../models/register-cashier-request.model';

@Injectable({
  providedIn: 'root',
})
export class AccountsService {
  headers = new HttpHeaders().set('Content-Type', 'application/json');
  loggedUser!: User;
  env = environment;

  jwtHelper: JwtHelperService = new JwtHelperService();

  get isLoggedIn(): Observable<boolean> {
    return of(this.getAccessToken() ? true : false);
  }

  get isUserAdmin(): Observable<boolean> {
    return of(this.isUserAdminCheck());
  }

  constructor(
    private http: HttpClient,
    private cookieService: CookieService,
    private router: Router
  ) {}

  getAccessToken() {
    return this.cookieService.get('access_token');
  }

  isUserAdminCheck(): boolean {
    const token = this.getAccessToken();

    const decodedToken = this.jwtHelper.decodeToken(token);

    return decodedToken.role.includes('Admin');
  }

  register(request: RegisterRequest) {
    return this.http
      .post<any>(REGISTER_ENDPOINT, request)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        this.router.navigate(['/resend-email'], {
          queryParams: { email: response.encodedEmail },
        });
      });
  }

  login(request: LoginRequest) {
    return this.http
      .post<LoginResponse>(LOGIN_ENDPOINT, request)
      .subscribe((response: LoginResponse) => {
        this.cookieService.set('access_token', response.accessToken, {
          path: '/',
          domain: this.env.cookieBaseDomain,
        });
        this.cookieService.set('refresh_token', response.refreshToken, {
          path: '/',
          domain: this.env.cookieBaseDomain,
        });
        this.cookieService.set('uid', response.id, {
          path: '/',
          domain: this.env.cookieBaseDomain,
        });

        this.router.navigate(['/user', response.id]);
      });
  }

  refreshAccessToken() {
    const refreshToken = this.cookieService.get('refresh_token');
    const userId = this.cookieService.get('uid');

    return this.http.get<RefreshAccessTokenResponse>(
      REFRESH_ACCESS_TOKEN_ENDPOINT,
      {
        params: {
          refreshToken: refreshToken,
          userId: userId,
        },
      }
    );
  }

  logout() {
    this.cookieService.deleteAll('/', this.env.cookieBaseDomain);
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

  confirmEmail(request: ConfirmEmailRequest) {
    return this.http.post(CONFIRM_EMAIL_ENDPOINT, request);
  }

  forgotPassword(request: ForgotPasswordRequest) {
    return this.http.post(FORGOT_PASSWORD_ENDPOINT, request);
  }

  resetPassword(request: ResetPasswordRequest) {
    return this.http
      .post<boolean>(RESET_PASSWORD_ENDPOINT, request)
      .pipe(catchError(this.handleError))
      .subscribe((response: boolean) => {
        if (response) {
          this.router.navigate(['/login']);
        }
      });
  }

  registerCashier(request: RegisterCashierRequest): Observable<boolean> {
    return this.http.post<boolean>(ADD_CASHIER_ENDPOINT, request);
  }

  getUser(userId: string): Observable<User> {
    return this.http.get<User>(GET_USER_ENDPOINT, {
      params: {
        userId: userId,
      },
    });
  }

  updateUser(userId: string, email: string, userName: string): Observable<any> {
    return this.http.put(UPDATE_USER_ENDPOINT, null, {
      params: {
        userId,
        email,
        userName,
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
