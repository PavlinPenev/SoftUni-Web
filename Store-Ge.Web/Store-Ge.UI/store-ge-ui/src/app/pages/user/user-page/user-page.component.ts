import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { filter, first } from 'rxjs';
import { Store } from 'src/app/models/store.model';
import { AccountsService } from 'src/app/services/accounts.service';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.scss'],
})
export class UserPageComponent implements OnInit {
  constants = constants;

  userStores: Store[] = [];
  isUserLoading: boolean = true;
  areStoresLoading: boolean = true;

  isExpanded: boolean = false;

  constructor(
    private accountsService: AccountsService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.accountsService
      .getUser(this.route.snapshot.params['userId'])
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        this.accountsService.loggedUser = response;
        this.isUserLoading = false;
      });
  }
}
