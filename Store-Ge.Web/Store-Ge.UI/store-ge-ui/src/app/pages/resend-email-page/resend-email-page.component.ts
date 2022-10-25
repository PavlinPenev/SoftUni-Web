import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import * as constants from '../../../assets/text.constants';

@Component({
  selector: 'app-resend-email-page',
  templateUrl: './resend-email-page.component.html',
  styleUrls: ['./resend-email-page.component.scss'],
})
export class ResendEmailPageComponent implements OnInit {
  constants = constants;
  email!: string;

  constructor(private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.email = params['email'];
    });
  }

  resendConfirmationEmail(): void {}
}
