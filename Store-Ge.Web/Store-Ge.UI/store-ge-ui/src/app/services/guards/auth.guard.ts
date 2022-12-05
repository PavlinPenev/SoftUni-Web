import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import {
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanActivate,
  Router,
} from '@angular/router';
import { Observable } from 'rxjs';
import * as constants from '../../../assets/text.constants';
import { AccountsService } from '../accounts.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constants = constants;

  constructor(
    public snackBar: MatSnackBar,
    public accountsService: AccountsService,
    public router: Router
  ) {}
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    if (!this.accountsService.getAccessToken()) {
      this.snackBar.open(constants.UNAUTHORIZED, constants.CLOSE, {
        horizontalPosition: 'center',
        verticalPosition: 'top',
      });
      this.router.navigate(['/login']);
      return false;
    }
    return true;
  }
}
