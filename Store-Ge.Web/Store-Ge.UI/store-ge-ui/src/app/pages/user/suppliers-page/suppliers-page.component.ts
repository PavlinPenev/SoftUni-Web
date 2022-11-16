import { Component, OnInit, ViewChild } from '@angular/core';
import { Location } from '@angular/common';
import * as constants from 'src/assets/text.constants';
import { Store } from 'src/app/models/store.model';
import { ActivatedRoute } from '@angular/router';
import { debounceTime, filter, first, Subject } from 'rxjs';
import { StoresService } from 'src/app/services/stores.service';
import { Sort } from '@angular/material/sort';
import { SuppliersResponse } from 'src/app/models/suppliers-response.model';
import { SuppliersService } from 'src/app/services/suppliers.service';
import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { SuppliersRequest } from 'src/app/models/suppliers-request.model';
import { MatPaginator } from '@angular/material/paginator';
import { MatBottomSheet } from '@angular/material/bottom-sheet';
import { AddSupplierComponent } from './add-supplier/add-supplier.component';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-suppliers-page',
  templateUrl: './suppliers-page.component.html',
  styleUrls: ['./suppliers-page.component.scss'],
})
export class SuppliersPageComponent implements OnInit {
  @ViewChild('paginator', { static: true }) paginator!: MatPaginator;
  dataSource = new MatTableDataSource();

  constants = constants;
  store!: Store;
  suppliers!: SuppliersResponse;

  userId: string = '';
  storeId: string = '';

  searchFormControl = new UntypedFormControl('');
  dateRangeFormControl: UntypedFormGroup = new UntypedFormGroup({
    start: new UntypedFormControl(''),
    end: new UntypedFormControl(''),
  });

  searchSubject = new Subject<string>();

  displayedColumns: string[] = ['name', 'createdOn'];

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

  initialSuppliersRequest: SuppliersRequest = {
    userId: '',
    searchTerm: '',
    orderBy: 'name',
    isDescending: false,
    dateAddedFrom: null,
    dateAddedTo: null,
    skip: 0,
    take: 5,
  };

  constructor(
    private location: Location,
    private route: ActivatedRoute,
    private storesService: StoresService,
    private suppliersService: SuppliersService,
    private addSupplierBottomSheet: MatBottomSheet
  ) {}

  ngOnInit(): void {
    this.searchSubject.pipe(debounceTime(800)).subscribe((searchTerm) => {
      this.search(searchTerm);
    });

    this.storeId = this.route.snapshot.params['storeId'];
    this.userId = this.route.snapshot.params['userId'];
    this.initialSuppliersRequest.userId = this.userId;
    this.suppliersRequest = { ...this.initialSuppliersRequest };

    this.storesService
      .getStore(this.storeId)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => (this.store = response));

    this.getSuppliers(this.suppliersRequest);
  }

  onAddSupplier(): void {
    this.addSupplierBottomSheet.open(AddSupplierComponent, {
      data: { userId: this.userId },
    });
  }

  onSortChange(e: Sort): void {
    this.suppliersRequest = {
      ...this.initialSuppliersRequest,
      isDescending: e.direction === 'desc',
    };

    this.getSuppliers(this.suppliersRequest);

    this.paginator.firstPage();
  }

  search(searchTerm: string): void {
    this.suppliersRequest = {
      ...this.initialSuppliersRequest,
      searchTerm: searchTerm,
    };

    this.getSuppliers(this.suppliersRequest);

    this.paginator.firstPage();
  }

  filterByDate(): void {
    this.suppliersRequest = {
      ...this.initialSuppliersRequest,
      dateAddedFrom: this.dateRangeFormControl.controls['start'].value,
      dateAddedTo: this.dateRangeFormControl.controls['end'].value,
    };

    this.getSuppliers(this.suppliersRequest);

    this.paginator.firstPage();
  }

  pageChanged(): void {
    this.suppliersRequest = {
      ...this.suppliersRequest,
      skip: this.paginator.pageIndex * this.paginator.pageSize,
      take: this.paginator.pageSize,
    };

    this.getSuppliers(this.suppliersRequest);
  }

  goBack(): void {
    this.location.back();
  }

  onKeyUp(): void {
    this.searchSubject.next(this.searchFormControl.value);
  }

  private getSuppliers(request: SuppliersRequest) {
    this.suppliersService
      .getUserSuppliersPaged(request)
      .pipe(
        filter((x) => !!x),
        first()
      )
      .subscribe((response) => {
        this.suppliers = response;

        this.dataSource.data = this.suppliers.items;
      });
  }
}
