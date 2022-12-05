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
import { JwtHelperService } from '@auth0/angular-jwt';
import { OrdersService } from 'src/app/services/orders.service';
import { OrdersRequest } from 'src/app/models/orders-request.model';
import { OrdersResponse } from 'src/app/models/orders-response.model';
import { SuppliersService } from 'src/app/services/suppliers.service';
import { SuppliersRequest } from 'src/app/models/suppliers-request.model';
import { SuppliersResponse } from 'src/app/models/suppliers-response.model';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-store-page',
  templateUrl: './store-page.component.html',
  styleUrls: ['./store-page.component.scss'],
})
export class StorePageComponent implements OnInit {
  @ViewChild('paginator', { static: true }) paginator!: MatPaginator;

  jwtHelper: JwtHelperService = new JwtHelperService();
  decodedToken: any = '';

  dataSource = new MatTableDataSource();

  searchField = new UntypedFormControl('');

  constants = constants;

  products: ProductsResponse = {
    items: [],
    totalItemsCount: 0,
  };
  orders: OrdersResponse = {
    items: [],
    totalItemsCount: 0,
  };
  suppliers: SuppliersResponse = {
    items: [],
    totalItemsCount: 0,
  };

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

  suppliersRequest: SuppliersRequest = {
    userId: '',
    searchTerm: '',
    orderBy: 'name',
    isDescending: false,
    dateAddedFrom: null,
    dateAddedTo: null,
    skip: 0,
    take: 5,
  };

  subject: Subject<string> = new Subject();

  constructor(
    private productsService: ProductsService,
    private ordersService: OrdersService,
    private storesService: StoresService,
    private suppliersService: SuppliersService,
    private route: ActivatedRoute,
    private router: Router,
    private location: Location,
    private cookieService: CookieService
  ) {}

  ngOnInit(): void {
    this.decodedToken = this.jwtHelper.decodeToken(
      this.cookieService.get('access_token')
    );

    this.userId = this.cookieService.get('uid');
    this.storeId = this.route.snapshot.params['storeId'];

    if (!this.decodedToken.role.includes('Admin')) {
      this.router.navigate([
        '/user',
        this.userId,
        'store',
        this.storeId,
        'sales',
      ]);
      return;
    }

    this.ordersRequest.storeId = this.storeId;
    this.suppliersRequest.userId = this.userId;

    this.subject.pipe(debounceTime(800)).subscribe((searchTextValue) => {
      this.searchByProductName(searchTextValue);
    });

    this.storesService
      .getStore(this.storeId)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.store = response));

    this.ordersService
      .getStoreOrders(this.ordersRequest)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.orders = response));

    this.suppliersService
      .getUserSuppliersPaged(this.suppliersRequest)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        this.suppliers = response;
      });

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
    this.storesService
      .exportReport(this.storeId)
      .pipe(first())
      .subscribe((response: HttpResponse<Blob>) => {
        var filename = '';
        var disposition = response.headers.get('Content-Disposition');
        if (disposition && disposition.indexOf('attachment') !== -1) {
          var filenameRegex = /filename[^;=\n]*=((['"]).*?\2|[^;\n]*)/;
          var matches = filenameRegex.exec(disposition);
          if (matches != null && matches[1])
            filename = matches[1].replace(/['"]/g, '');
        }
        let a = document.createElement('a');
        a.href = window.URL.createObjectURL(response.body!);
        a.download = filename;
        document.body.append(a);
        a.click();
      });
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

  navigateToSalesPage(): void {
    this.router.navigate([
      '/user',
      this.userId,
      'store',
      this.storeId,
      'sales',
    ]);
  }

  onKeyUp(): void {
    this.subject.next(this.searchField.value);
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
