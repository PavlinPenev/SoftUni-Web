import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Location } from '@angular/common';
import * as constants from 'src/assets/text.constants';
import { Sort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { OrdersResponse } from 'src/app/models/orders-response.model';
import { Store } from 'src/app/models/store.model';
import { ActivatedRoute } from '@angular/router';
import { StoresService } from 'src/app/services/stores.service';
import { debounceTime, filter, first, Subject, Subscription } from 'rxjs';
import { OrdersService } from 'src/app/services/orders.service';
import { OrdersRequest } from 'src/app/models/orders-request.model';
import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { AddOrderComponent } from './add-order/add-order.component';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-orders-page',
  templateUrl: './orders-page.component.html',
  styleUrls: ['./orders-page.component.scss'],
})
export class OrdersPageComponent implements OnInit, OnDestroy {
  @ViewChild('paginator', { static: true }) paginator!: MatPaginator;
  dataSource = new MatTableDataSource();

  searchFormControl = new UntypedFormControl('');

  dateRangeFormControl: UntypedFormGroup = new UntypedFormGroup({
    start: new UntypedFormControl(''),
    end: new UntypedFormControl(''),
  });

  constants = constants;
  store!: Store;
  storeId: string = '';
  userId: string = '';
  orders: OrdersResponse = {
    items: [],
    totalItemsCount: 0,
  };

  ordersRequest: OrdersRequest = {
    storeId: '',
    searchTerm: '',
    orderBy: 'orderNumber',
    isDescending: false,
    dateAddedFrom: null,
    dateAddedTo: null,
    skip: 0,
    take: 5,
  };

  initialOrdersRequest: OrdersRequest = {
    storeId: '',
    searchTerm: '',
    orderBy: 'orderNumber',
    isDescending: false,
    dateAddedFrom: null,
    dateAddedTo: null,
    skip: 0,
    take: 5,
  };

  displayedColumns: string[] = ['orderNumber', 'supplierName', 'createdOn'];

  subs: Subscription[] = [];
  subject: Subject<string> = new Subject();

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private storesService: StoresService,
    private ordersService: OrdersService,
    private addOrderBottomSheet: MatBottomSheet
  ) {}

  ngOnInit(): void {
    this.subject.pipe(debounceTime(800)).subscribe((searchTextValue) => {
      this.search(searchTextValue);
    });

    this.storeId = this.route.snapshot.params['storeId'];
    this.userId = this.route.snapshot.params['userId'];
    this.initialOrdersRequest.storeId = this.storeId;
    this.ordersRequest = { ...this.initialOrdersRequest };
    this.storesService
      .getStore(this.storeId)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.store = response));

    this.subs.push(
      this.ordersService
        .getStoreOrders(this.initialOrdersRequest)
        .pipe(
          filter((x) => !!x),
          first()
        )
        .subscribe((response) => {
          this.orders = response;

          this.dataSource.data = this.orders.items;
        })
    );
  }

  ngOnDestroy(): void {
    this.subs.forEach((x) => x.unsubscribe());
  }

  goBack(): void {
    this.location.back();
  }

  onSortChange(e: Sort): void {
    this.ordersRequest = {
      ...this.initialOrdersRequest,
      isDescending: e.direction === 'desc',
    };

    this.getStoreOrders(this.ordersRequest);

    this.paginator.firstPage();
  }

  pageChanged(): void {
    this.ordersRequest = {
      ...this.ordersRequest,
      skip: this.paginator.pageIndex * this.paginator.pageSize,
      take: this.paginator.pageSize,
    };

    this.getStoreOrders(this.ordersRequest);
  }

  search(searchTerm: string): void {
    this.ordersRequest = {
      ...this.initialOrdersRequest,
      searchTerm: searchTerm,
    };

    this.getStoreOrders(this.ordersRequest);

    this.paginator.firstPage();
  }

  filterByDate(): void {
    this.ordersRequest = {
      ...this.initialOrdersRequest,
      dateAddedFrom: this.dateRangeFormControl.controls['start'].value,
      dateAddedTo: this.dateRangeFormControl.controls['end'].value,
    };

    this.getStoreOrders(this.ordersRequest);

    this.paginator.firstPage();
  }

  onKeyUp(): void {
    this.subject.next(this.searchFormControl.value);
  }

  onAddOrder(): void {
    const storeId = this.storeId;
    const userId = this.userId;

    this.addOrderBottomSheet.open(AddOrderComponent, {
      data: { storeId, userId },
    });
  }

  private getStoreOrders(request: OrdersRequest) {
    this.ordersService
      .getStoreOrders(request)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.orders = response));
  }
}
