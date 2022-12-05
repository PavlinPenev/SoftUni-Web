import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddSupplierRequest } from '../models/add-supplier-request.model';
import { Supplier } from '../models/supplier.model';
import { SuppliersRequest } from '../models/suppliers-request.model';
import { SuppliersResponse } from '../models/suppliers-response.model';
import {
  ADD_SUPPLIER_ENDPOINT,
  GET_USER_SUPPLIERS_ENDPOINT,
  GET_USER_SUPPLIERS_PAGED_ENDPOINT,
} from '../shared/api-endpoints';

@Injectable({
  providedIn: 'root',
})
export class SuppliersService {
  constructor(private http: HttpClient) {}

  getUserSuppliers(userId: string): Observable<Supplier[]> {
    return this.http.get<Supplier[]>(GET_USER_SUPPLIERS_ENDPOINT, {
      params: {
        userId,
      },
    });
  }

  getUserSuppliersPaged(
    request: SuppliersRequest
  ): Observable<SuppliersResponse> {
    return this.http.post<SuppliersResponse>(
      GET_USER_SUPPLIERS_PAGED_ENDPOINT,
      request
    );
  }

  addSupplier(request: AddSupplierRequest): Observable<any> {
    return this.http.post(ADD_SUPPLIER_ENDPOINT, request);
  }
}
