import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { ActivatedRoute } from '@angular/router';
import { filter, first, Subscription } from 'rxjs';
import { Order } from 'src/app/models/order.model';
import { ProductsResponse } from 'src/app/models/products-response.model';
import { StoreProductsRequest } from 'src/app/models/store-products-request.model';
import { StoreTypeEnum } from 'src/app/models/store-type.enum';
import { Store } from 'src/app/models/store.model';
import { Supplier } from 'src/app/models/supplier.model';
import { ProductsService } from 'src/app/services/products.service';
import { StoresService } from 'src/app/services/stores.service';
import * as constants from 'src/assets/text.constants';

@Component({
  selector: 'app-store-page',
  templateUrl: './store-page.component.html',
  styleUrls: ['./store-page.component.scss'],
})
export class StorePageComponent implements OnInit, OnDestroy {
  @ViewChild('paginator', { static: true }) paginator!: MatPaginator;

  constants = constants;

  products!: ProductsResponse;
  orders: Order[] = [];
  suppliers: Supplier[] = [];
  store!: Store;
  storeId!: string;

  subs: Subscription[] = [];

  displayedColumns: string[] = ['name', 'price', 'quantity', 'measurementUnit'];
  productsRequest: StoreProductsRequest = {
    storeId: '',
    searchTerm: '',
    isDescending: false,
    skip: 0,
    take: 10,
  };

  initialProductsRequest: StoreProductsRequest = {
    storeId: '',
    searchTerm: '',
    isDescending: false,
    skip: 0,
    take: 10,
  };

  constructor(
    private productsService: ProductsService,
    private storesService: StoresService,
    private route: ActivatedRoute,
    private location: Location
  ) {}

  ngOnInit(): void {
    this.storeId = this.route.snapshot.params['storeId'];

    this.storesService
      .getStore(this.storeId)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.store = response));

    this.productsRequest.storeId = this.storeId;

    this.subs.push(
      this.productsService
        .getStoreProducts(this.productsRequest)
        .pipe(
          filter((x) => !!x),
          first()
        )
        .subscribe((response) => (this.products = response))
    );
  }

  getStoreTypeString(type: StoreTypeEnum) {
    return StoreTypeEnum[type];
  }

  exportExcel(): void {
    // TODO
  }

  onSortChange(e: Sort): void {
    this.productsRequest = {
      ...this.initialProductsRequest,
      storeId: this.storeId,
      isDescending: e.direction === 'desc',
    };

    this.productsService
      .getStoreProducts(this.productsRequest)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.products = response));

    this.paginator.firstPage();
  }

  pageChanged(e: PageEvent): void {
    const previousPageIndex = e.previousPageIndex! + 1;
    const pageIndex = e.pageIndex + 1;

    this.productsRequest = {
      ...this.productsRequest,
      skip: this.paginator.pageIndex * this.paginator.pageSize,
      take: this.paginator.pageSize,
    };
  }

  searchByProductName(event: any): void {
    this.paginator.firstPage();

    this.productsRequest = {
      ...this.initialProductsRequest,
      searchTerm: event.target.value,
    };

    this.productsService
      .getStoreProducts(this.productsRequest)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.products = response));
  }

  goBack(): void {
    this.location.back();
  }

  ngOnDestroy(): void {
    this.subs.forEach((x) => x.unsubscribe());
  }
}
