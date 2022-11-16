import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  userId: string = '';

  userStores: Store[] = [];
  isUserLoading: boolean = true;
  areStoresLoading: boolean = true;

  isExpanded: boolean = false;

  constructor(
    private accountsService: AccountsService,
    private route: ActivatedRoute,
    private router: Router
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
        this.accountsService.loggedUser = response;
        this.isUserLoading = false;
      });
  }

  navigateToAllOrders(): void {
    this.router.navigate(['/user', this.userId, 'all-orders']);
  }
}
