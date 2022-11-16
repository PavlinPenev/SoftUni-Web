import { Location } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { debounceTime, filter, first, Subject } from 'rxjs';
import { MeasurementUnitEnum } from 'src/app/models/measurement-unit.enum';
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
export class StorePageComponent implements OnInit {
  @ViewChild('paginator', { static: true }) paginator!: MatPaginator;
  dataSource = new MatTableDataSource();

  searchField = new UntypedFormControl('');

  constants = constants;

  products: ProductsResponse = {
    items: [],
    totalItemsCount: 0,
  };
  orders: Order[] = [];
  suppliers: Supplier[] = [];
  store!: Store;
  storeId!: string;
  userId!: string;

  displayedColumns: string[] = ['name', 'price', 'quantity', 'measurementUnit'];
  productsRequest: StoreProductsRequest = {
    storeId: '',
    searchTerm: '',
    isDescending: false,
    skip: 0,
    take: 5,
  };

  initialProductsRequest: StoreProductsRequest = {
    storeId: '',
    searchTerm: '',
    isDescending: false,
    skip: 0,
    take: 5,
  };

  subject: Subject<string> = new Subject();

  constructor(
    private productsService: ProductsService,
    private storesService: StoresService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private cookieService: CookieService
  ) {}

  ngOnInit(): void {
    this.subject.pipe(debounceTime(800)).subscribe((searchTextValue) => {
      this.searchByProductName(searchTextValue);
    });

    this.userId = this.cookieService.get('uid');
    this.storeId = this.route.snapshot.params['storeId'];

    this.storesService
      .getStore(this.storeId)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.store = response));

    this.initialProductsRequest.storeId = this.storeId;
    this.productsRequest = {
      ...this.initialProductsRequest,
    };

    this.getProducts(this.productsRequest);
  }

  getStoreTypeString(type: StoreTypeEnum) {
    return StoreTypeEnum[type];
  }

  getUnitTypeString(measurementUnit: MeasurementUnitEnum) {
    return MeasurementUnitEnum[measurementUnit];
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

    this.getProducts(this.productsRequest);

    this.paginator.firstPage();
  }

  pageChanged(): void {
    this.productsRequest = {
      ...this.productsRequest,
      skip: this.paginator.pageIndex * this.paginator.pageSize,
      take: this.paginator.pageSize,
    };

    this.getProducts(this.productsRequest);
  }

  searchByProductName(searchTerm: string): void {
    this.paginator.firstPage();

    this.productsRequest = {
      ...this.initialProductsRequest,
      searchTerm: searchTerm,
    };

    this.getProducts(this.productsRequest);
  }

  goBack(): void {
    this.location.back();
  }

  navigateToOrdersPage(): void {
    this.router.navigate([
      '/user',
      this.userId,
      'store',
      this.storeId,
      'orders',
    ]);
  }

  navigateToSuppliersPage(): void {
    this.router.navigate([
      '/user',
      this.userId,
      'store',
      this.storeId,
      'suppliers',
    ]);
  }

  onKeyUp(): void {
    const searchTerm = this.searchField.value;
  }

  private getProducts(request: StoreProductsRequest) {
    this.productsService
      .getStoreProducts(this.productsRequest)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        this.products = response;

        this.dataSource.data = this.products.items;
      });
  }
}
