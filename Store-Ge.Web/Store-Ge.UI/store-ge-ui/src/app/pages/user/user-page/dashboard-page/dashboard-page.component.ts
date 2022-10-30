import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { filter, first } from 'rxjs';
import { Order } from 'src/app/models/order.model';
import { Store } from 'src/app/models/store.model';
import { Supplier } from 'src/app/models/supplier.model';
import { StoresService } from 'src/app/services/stores.service';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-dashboard-page',
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.scss'],
})
export class DashboardPageComponent implements OnInit {
  constants = constants;

  userStores: Store[] = [];
  suppliers: Supplier[] = [];
  orders: Order[] = [];
  areStoresLoading: boolean = true;

  constructor(
    private storesService: StoresService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.storesService
      .getUserStores(this.route.snapshot.params['userId'])
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        this.userStores = response;
        this.areStoresLoading = false;
      });
  }
}
