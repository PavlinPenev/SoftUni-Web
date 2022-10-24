import { Component, OnInit } from '@angular/core';
import { AbstractControl, UntypedFormControl, UntypedFormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import * as textConstants from '../../../assets/text.constants';


@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss']
})
export class RegisterPageComponent implements OnInit {
  constants = textConstants;

  showPassword = false;
  showConfirmPassword = false;

  private passwordMatchValidator: ValidatorFn = (formGroup: AbstractControl): ValidationErrors | null => {
    if (formGroup.get('password')?.value === formGroup.get('confirmPassword')?.value)
      return null;
    else
      return {passwordMismatch: true};
  };

  form = new UntypedFormGroup({
    username: new UntypedFormControl('', Validators.required),
    email: new UntypedFormControl('',
            [Validators.required,
            Validators.email]),
    password: new UntypedFormControl('', 
            [Validators.required,
            Validators.pattern(this.constants.PASSWORD_VALIDATION_PATTERN)]),
    confirmPassword: new UntypedFormControl('', Validators.required),
    acceptanceCheckbox: new UntypedFormControl(false, Validators.requiredTrue)
  }, {validators:  this.passwordMatchValidator});

  get password() { return this.form.get('password'); }
  get confirmPassword() { return this.form.get('confirmPassword'); }

  constructor() { }

  ngOnInit(): void {
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
	this.showConfirmPassword = !this.showConfirmPassword;
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
