import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import * as textConstants from '../../../assets/text.constants';

@Component({
  selector: 'app-login-page',
  templateUrl: './login-page.component.html',
  styleUrls: ['./login-page.component.scss']
})
export class LoginPageComponent implements OnInit {
  constants = textConstants;

  form = new FormGroup({
    email: new FormControl('',
            [Validators.required,
            Validators.email]),
    password: new FormControl('', 
            [Validators.required,
            Validators.pattern(this.constants.PASSWORD_VALIDATION_PATTERN)])
  });

  constructor() { }

  ngOnInit(): void {
  }
}
