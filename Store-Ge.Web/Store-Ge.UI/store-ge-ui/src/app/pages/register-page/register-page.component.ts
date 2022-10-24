import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import * as textConstants from '../../../assets/text.constants';


@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss']
})
export class RegisterPageComponent implements OnInit {
  constants = textConstants;

  private passwordMatchValidator: ValidatorFn = (formGroup: AbstractControl): ValidationErrors | null => {
    if (formGroup.get('password')?.value === formGroup.get('confirmPassword')?.value)
      return null;
    else
      return {passwordMismatch: true};
  };

  form = new FormGroup({
    username: new FormControl('', Validators.required),
    email: new FormControl('',
            [Validators.required,
            Validators.email]),
    password: new FormControl('', 
            [Validators.required,
            Validators.pattern(this.constants.PASSWORD_VALIDATION_PATTERN)]),
    confirmPassword: new FormControl('', Validators.required),
    acceptanceCheckbox: new FormControl(false, Validators.requiredTrue)
  }, {validators:  this.passwordMatchValidator});

  get password() { return this.form.get('password'); }
  get confirmPassword() { return this.form.get('confirmPassword'); }

  constructor() { }

  ngOnInit(): void {
  }

  onPasswordInput(){
    if (this.form.hasError('passwordMismatch')){
    this.confirmPassword?.setErrors([{'passwordMismatch': true}]);
    }
    else{
      this.confirmPassword?.setErrors(null);
    }
  }
}
