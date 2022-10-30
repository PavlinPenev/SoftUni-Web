import { Component, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ActivatedRoute } from '@angular/router';
import { filter, first } from 'rxjs';
import { ConfirmEmailRequest } from 'src/app/models/confirm-email.model';
import { AccountsService } from 'src/app/services/accounts.service';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-confirm-email-page',
  templateUrl: './confirm-email-page.component.html',
  styleUrls: ['./confirm-email-page.component.scss'],
})
export class ConfirmEmailPageComponent implements OnInit {
  constants = constants;
  emailToken!: string;
  userId!: string;

  constructor(
    private route: ActivatedRoute,
    private accountsService: AccountsService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.emailToken = params['emailToken'];
      this.userId = params['userId'];
    });

    this.confirmEmail();
  }

  confirmEmail(): void {
    const request: ConfirmEmailRequest = {
      userId: this.userId,
      emailToken: this.emailToken,
    };

    this.accountsService
      .confirmEmail(request)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        if (!response) {
          this.snackBar.open(constants.SOMETHING_WENT_WRONG, constants.CLOSE, {
            horizontalPosition: 'center',
            verticalPosition: 'top',
          });
        }
      });
  }
}
