import { Component, OnInit } from '@angular/core';
import {
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { ForgotPasswordRequest } from 'src/app/models/forgot-password.model';
import { AccountsService } from 'src/app/services/accounts.service';
import * as constants from 'src/assets/text.constants';
@Component({
  selector: 'app-forgotten-page',
  templateUrl: './forgotten-page.component.html',
  styleUrls: ['./forgotten-page.component.scss'],
})
export class ForgottenPageComponent implements OnInit {
  constants = constants;

  form = new UntypedFormGroup({
    email: new UntypedFormControl('', [Validators.required, Validators.email]),
  });

  constructor(private accountsService: AccountsService) {}

  ngOnInit(): void {}

  forgotPassword(): void {
    const request: ForgotPasswordRequest = {
      email: this.form.get('email')?.value,
    };

    this.accountsService.forgotPassword(request);
  }
}
