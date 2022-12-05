import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AccountsService } from 'src/app/services/accounts.service';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-resend-email-page',
  templateUrl: './resend-email-page.component.html',
  styleUrls: ['./resend-email-page.component.scss'],
})
export class ResendEmailPageComponent implements OnInit {
  constants = constants;
  email!: string;

  constructor(
    private route: ActivatedRoute,
    private accountsService: AccountsService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.email = params['email'];
    });
  }

  resendConfirmationEmail(): void {
    this.accountsService.resendConfirmationEmail(this.email);
  }
}
