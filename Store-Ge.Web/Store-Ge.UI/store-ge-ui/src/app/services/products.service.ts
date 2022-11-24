import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddProduct } from '../models/add-product.model';
import { ProductsResponse } from '../models/products-response.model';
import { SaleRequest } from '../models/sale-request.model';
import { StoreProductsRequest } from '../models/store-products-request.model';
import {
  GET_STORE_ADD_PRODUCTS_ENDPOINT,
  GET_STORE_PRODUCTS_ENDPOINT,
  SELL_PRODUCTS_ENDPOINT,
} from '../shared/api-endpoints';

@Injectable({
  providedIn: 'root',
})
export class ProductsService {
  constructor(private http: HttpClient) {}

  getStoreProducts(
    request: StoreProductsRequest
  ): Observable<ProductsResponse> {
    return this.http.post<ProductsResponse>(
      GET_STORE_PRODUCTS_ENDPOINT,
      request
    );
  }

  getStoreAddProducts(storeId: string): Observable<AddProduct[]> {
    return this.http.get<AddProduct[]>(GET_STORE_ADD_PRODUCTS_ENDPOINT, {
      params: { storeId },
    });
  }

  sellProducts(request: SaleRequest): Observable<boolean> {
    return this.http.post<boolean>(SELL_PRODUCTS_ENDPOINT, request);
  }
}
