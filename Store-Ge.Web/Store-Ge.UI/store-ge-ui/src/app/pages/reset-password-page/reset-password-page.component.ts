import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  UntypedFormControl,
  UntypedFormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { ResetPasswordRequest } from 'src/app/models/reset-password.model';
import { AccountsService } from 'src/app/services/accounts.service';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-reset-password-page',
  templateUrl: './reset-password-page.component.html',
  styleUrls: ['./reset-password-page.component.scss'],
})
export class ResetPasswordPageComponent implements OnInit {
  constants = constants;

  userEmail!: string;
  resetPasswordToken!: string;

  showPassword = false;
  showConfirmPassword = false;

  private passwordMatchValidator: ValidatorFn = (
    formGroup: AbstractControl
  ): ValidationErrors | null => {
    if (
      formGroup.get('password')?.value ===
      formGroup.get('confirmPassword')?.value
    )
      return null;
    else return { passwordMismatch: true };
  };

  form = new UntypedFormGroup(
    {
      password: new UntypedFormControl('', [
        Validators.required,
        Validators.pattern(this.constants.PASSWORD_VALIDATION_PATTERN),
      ]),
      confirmPassword: new UntypedFormControl('', Validators.required),
    },
    { validators: this.passwordMatchValidator }
  );

  get password() {
    return this.form.get('password');
  }
  get confirmPassword() {
    return this.form.get('confirmPassword');
  }

  constructor(
    private route: ActivatedRoute,
    private accountsService: AccountsService
  ) {}

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      this.userEmail = params['userEmail'];
      this.resetPasswordToken = params['passwordToken'];
    });
  }

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  onPasswordInput(): void {
    if (this.form.hasError('passwordMismatch')) {
      this.confirmPassword?.setErrors([{ passwordMismatch: true }]);
    } else {
      this.confirmPassword?.setErrors(null);
    }
  }

  resetPassword(): void {
    const request: ResetPasswordRequest = {
      email: this.userEmail,
      password: this.form.get('password')?.value,
      confirmPassword: this.form.get('confirmPassword')?.value,
      resetPasswordToken: this.resetPasswordToken,
    };

    this.accountsService.resetPassword(request);
  }
}
