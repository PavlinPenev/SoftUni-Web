import { Component, OnInit, ViewChild } from '@angular/core';
import { Location } from '@angular/common';
import * as constants from 'src/assets/text.constants';
import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute } from '@angular/router';
import { debounceTime, filter, first, Subject } from 'rxjs';
import { AllOrdersResponse } from 'src/app/models/all-orders-response.model';
import { AllOrdersRequest } from 'src/app/models/all-orders-request.model';
import { MatPaginator } from '@angular/material/paginator';
import { OrdersService } from 'src/app/services/orders.service';

@Component({
  selector: 'app-all-orders',
  templateUrl: './all-orders.component.html',
  styleUrls: ['./all-orders.component.scss'],
})
export class AllOrdersComponent implements OnInit {
  @ViewChild('paginator', { static: true }) paginator!: MatPaginator;

  constants = constants;
  dataSource = new MatTableDataSource();
  orders!: AllOrdersResponse;

  userId: string = '';

  searchFormControl = new UntypedFormControl('');
  dateRangeFormControl: UntypedFormGroup = new UntypedFormGroup({
    start: new UntypedFormControl(''),
    end: new UntypedFormControl(''),
  });

  searchSubject: Subject<string> = new Subject<string>();

  ordersRequest: AllOrdersRequest = {
    userId: '',
    searchTerm: '',
    orderBy: 'orderNumber',
    isDescending: false,
    dateAddedFrom: null,
    dateAddedTo: null,
    skip: 0,
    take: 5,
  };

  initialOrdersRequest: AllOrdersRequest = {
    userId: '',
    searchTerm: '',
    orderBy: 'orderNumber',
    isDescending: false,
    dateAddedFrom: null,
    dateAddedTo: null,
    skip: 0,
    take: 5,
  };

  displayedColumns: string[] = ['orderNumber', 'storeName', 'createdOn'];

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private ordersService: OrdersService
  ) {}

  ngOnInit(): void {
    this.searchSubject
      .pipe(debounceTime(800))
      .subscribe((searchTerm: string) => {
        this.search(searchTerm);
      });

    this.userId = this.route.snapshot.params['userId'];
    this.initialOrdersRequest.userId = this.userId;
    this.ordersRequest = {
      ...this.initialOrdersRequest,
    };

    this.getAllOrders(this.ordersRequest);
  }

  onSortChange(e: Sort): void {
    this.ordersRequest = {
      ...this.initialOrdersRequest,
      isDescending: e.direction === 'desc',
    };

    this.getAllOrders(this.ordersRequest);

    this.paginator.firstPage();
  }

  filterByDate(): void {
    this.ordersRequest = {
      ...this.initialOrdersRequest,
      dateAddedFrom: this.dateRangeFormControl.controls['start'].value,
      dateAddedTo: this.dateRangeFormControl.controls['end'].value,
    };

    this.getAllOrders(this.ordersRequest);

    this.paginator.firstPage();
  }

  pageChanged(): void {
    this.ordersRequest = {
      ...this.ordersRequest,
      skip: this.paginator.pageIndex * this.paginator.pageSize,
      take: this.paginator.pageSize,
    };

    this.getAllOrders(this.ordersRequest);
  }

  onKeyUp(): void {
    this.searchSubject.next(this.searchFormControl.value);
  }

  search(searchTerm: string): void {
    this.ordersRequest = {
      ...this.initialOrdersRequest,
      searchTerm: searchTerm,
    };

    this.getAllOrders(this.ordersRequest);

    this.paginator.firstPage();
  }

  goBack(): void {
    this.location.back();
  }

  private getAllOrders(request: AllOrdersRequest): void {
    this.ordersService
      .getUserOrders(request)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        this.orders = response;

        this.dataSource.data = this.orders.items;
      });
  }
}
