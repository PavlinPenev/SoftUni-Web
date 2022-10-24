import { Component, OnInit } from '@angular/core';
import { UntypedFormControl, UntypedFormGroup, Validators } from '@angular/forms';
import * as textConstants from '../../../assets/text.constants';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {
  constants = textConstants;
  showPassword: boolean = false;

  form = new UntypedFormGroup({
    email: new UntypedFormControl('',
            [Validators.required,
            Validators.email]),
    password: new UntypedFormControl('', 
            [Validators.required,
            Validators.pattern(this.constants.PASSWORD_VALIDATION_PATTERN)])
  });

  constructor() { }

  ngOnInit(): void {
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }
}
