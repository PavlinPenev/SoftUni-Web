import { Component, Inject, OnInit } from '@angular/core';
import {
  AbstractControl,
  UntypedFormControl,
  UntypedFormGroup,
  ValidationErrors,
  ValidatorFn,
  Validators,
} from '@angular/forms';
import {
  MatBottomSheetRef,
  MAT_BOTTOM_SHEET_DATA,
} from '@angular/material/bottom-sheet';
import { filter, first } from 'rxjs';
import { RegisterCashierRequest } from 'src/app/models/register-cashier-request.model';
import { AccountsService } from 'src/app/services/accounts.service';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-add-cashier',
  templateUrl: './add-cashier.component.html',
  styleUrls: ['./add-cashier.component.scss'],
})
export class AddCashierComponent implements OnInit {
  constants = constants;

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
    private accountsService: AccountsService,
    private bottomSheetRef: MatBottomSheetRef<AddCashierComponent>,
    @Inject(MAT_BOTTOM_SHEET_DATA)
    public data: { storeId: string }
  ) {}

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

  registerCashier(): void {
    const request: RegisterCashierRequest = {
      storeId: this.data.storeId,
      userName: this.form.value.username!,
      email: this.form.value.email!,
      password: this.form.value.password!,
      confirmPassword: this.form.value.confirmPassword!,
    };

    this.accountsService
      .registerCashier(request)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe();

    this.bottomSheetRef.dismiss();
  }
}
