import { Component, OnInit } from '@angular/core';
import {
  UntypedFormControl,
  UntypedFormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, first } from 'rxjs';
import { User } from 'src/app/models/user.model';
import { AccountsService } from 'src/app/services/accounts.service';
import * as textConstants from 'src/assets/text.constants';
import { Location } from '@angular/common';

@Component({
  selector: 'app-account-settings-page',
  templateUrl: './account-settings-page.component.html',
  styleUrls: ['./account-settings-page.component.scss'],
})
export class AccountSettingsPageComponent implements OnInit {
  constants = textConstants;
  showPassword: boolean = false;
  userId: string = '';
  user!: User;

  form = new UntypedFormGroup({
    email: new UntypedFormControl('', [Validators.required, Validators.email]),
    userName: new UntypedFormControl('', Validators.required),
  });

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private accountsService: AccountsService,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.userId = this.route.snapshot.params['userId'];
    this.accountsService
      .getUser(this.userId)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        this.user = response;

        this.form.controls['email'].setValue(this.user.email);
        this.form.controls['userName'].setValue(this.user.userName);
      });
  }

  updateUser(): void {
    const email = this.form.controls['email'].value;
    const userName = this.form.controls['userName'].value;

    this.accountsService
      .updateUser(this.userId, email, userName)
      .pipe(first())
      .subscribe((response) => {
        this.router.navigate(['/user', this.userId]);
      });
  }

  goBack(): void {
    this.location.back();
  }
}
