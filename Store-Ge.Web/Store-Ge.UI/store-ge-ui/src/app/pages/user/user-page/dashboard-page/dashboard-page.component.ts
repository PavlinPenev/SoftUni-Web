import { Component, OnInit } from '@angular/core';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, first } from 'rxjs';
import { Order } from 'src/app/models/order.model';
import { StoreTypeEnum } from 'src/app/models/store-type.enum';
import { Store } from 'src/app/models/store.model';
import { Supplier } from 'src/app/models/supplier.model';
import { StoresService } from 'src/app/services/stores.service';
import * as constants from 'src/assets/text.constants';
import { AddStoreModalComponent } from './add-store-modal/add-store-modal.component';

@Component({
  selector: 'app-dashboard-page',
  templateUrl: './dashboard-page.component.html',
  styleUrls: ['./dashboard-page.component.scss'],
})
export class DashboardPageComponent implements OnInit {
  constants = constants;
  userId: string = '';

  userStores: Store[] = [];
  areStoresLoading: boolean = true;

  constructor(
    private storesService: StoresService,
    private route: ActivatedRoute,
    private router: Router,
    private addStoreBottomSheet: MatBottomSheet
  ) {}

  ngOnInit(): void {
    this.userId = this.route.snapshot.params['userId'];

    this.storesService
      .getUserStores(this.userId)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        this.userStores = response;
        this.areStoresLoading = false;
      });
  }
  addStore(): void {
    this.addStoreBottomSheet.open(AddStoreModalComponent);
  }

  goToStorePage(store: Store): void {
    this.router.navigate(['/user', this.userId, 'store', store.id]);
  }

  isEmptyStateVisible() {
    return eval('this.userStores.length === 0');
  }

  isStoreCardContainerVisible() {
    return eval('this.userStores.length > 0');
  }

  getStoreTypeString(type: StoreTypeEnum) {
    return StoreTypeEnum[type];
  }
}
