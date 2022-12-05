import { Component, OnInit } from '@angular/core';
import {
  AbstractControl,
  UntypedFormControl,
  UntypedFormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import { RegisterRequest } from 'src/app/models/register-request.model';
import { AccountsService } from 'src/app/services/accounts.service';
import * as textConstants from 'src/assets/text.constants';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss'],
})
export class RegisterPageComponent implements OnInit {
  constants = textConstants;

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
      username: new UntypedFormControl('', Validators.required),
      email: new UntypedFormControl('', [
        Validators.required,
        Validators.email,
      ]),
      password: new UntypedFormControl('', [
        Validators.required,
        Validators.pattern(this.constants.PASSWORD_VALIDATION_PATTERN),
      ]),
      confirmPassword: new UntypedFormControl('', Validators.required),
      acceptanceCheckbox: new UntypedFormControl(
        false,
        Validators.requiredTrue
      ),
    },
    { validators: this.passwordMatchValidator }
  );

  get password() {
    return this.form.get('password');
  }
  get confirmPassword() {
    return this.form.get('confirmPassword');
  }

  constructor(private accountsService: AccountsService) {}

  ngOnInit(): void {}

  togglePasswordVisibility(): void {
    this.showPassword = !this.showPassword;
  }

  toggleConfirmPasswordVisibility(): void {
    this.showConfirmPassword = !this.showConfirmPassword;
  }

  onPasswordInput() {
    if (this.form.hasError('passwordMismatch')) {
      this.confirmPassword?.setErrors([{ passwordMismatch: true }]);
    } else {
      this.confirmPassword?.setErrors(null);
    }
  }

  register(): void {
    const request: RegisterRequest = {
      userName: this.form.value.username!,
      email: this.form.value.email!,
      password: this.form.value.password!,
      confirmPassword: this.form.value.confirmPassword!,
    };

    this.accountsService.register(request);
  }

  openTAC(): void {
    window.open(
      '../../../assets/terms-and-conditions-page.html',
      '_blank',
      'status=0,scrollbars=1,resizable=1,location=1'
    );
  }
}
