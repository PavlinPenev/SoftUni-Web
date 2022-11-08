import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpErrorResponse,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { AccountsService } from './accounts.service';
import { catchError, switchMap, throwError } from 'rxjs';
import { CookieService } from 'ngx-cookie-service';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  isRefreshing: boolean = false;

  constructor(
    private accountsService: AccountsService,
    private cookieService: CookieService
  ) {}

  intercept(request: HttpRequest<any>, next: HttpHandler) {
    const accessToken = this.accountsService.getAccessToken();
    request = request.clone({
      setHeaders: {
        Authorization: 'Bearer ' + accessToken,
      },
    });
    return next.handle(request).pipe(
      catchError((error) => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          return this.handle401Error(request, next);
        }

        return throwError(() => error);
      })
    );
  }

  private handle401Error(request: HttpRequest<any>, next: HttpHandler) {
    if (!this.isRefreshing) {
      this.isRefreshing = true;

      if (this.accountsService.isLoggedIn) {
        return this.accountsService.refreshAccessToken().pipe(
          switchMap((response) => {
            this.isRefreshing = false;
            this.cookieService.set('access_token', response.accessToken);
            this.cookieService.set('refresh_token', response.refreshToken);

            return next.handle(request);
          }),
          catchError((error) => {
            this.isRefreshing = false;

            if (error.status == '403') {
              this.accountsService.logout();
            }

            return throwError(() => error);
          })
        );
      }
    }

    return next.handle(request);
  }
}

export const httpInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
];
