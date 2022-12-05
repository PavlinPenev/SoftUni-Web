import { Component, OnInit } from '@angular/core';
import {
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { LoginRequest } from 'src/app/models/login-request.model';
import { AccountsService } from 'src/app/services/accounts.service';
import * as textConstants from 'src/assets/text.constants';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss'],
})
export class LoginPageComponent implements OnInit {
  constants = textConstants;
  showPassword: boolean = false;

  form = new UntypedFormGroup({
    email: new UntypedFormControl('', [Validators.required, Validators.email]),
    password: new UntypedFormControl('', [
      Validators.required,
      Validators.pattern(this.constants.PASSWORD_VALIDATION_PATTERN),
    ]),
  });

  constructor(private accountsService: AccountsService) {}

  ngOnInit(): void {}

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  login(): void {
    const request: LoginRequest = {
      email: this.form.value.email!,
      password: this.form.value.password!,
    };

    this.accountsService.login(request);
  }
}
