import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from '../models/product.model';
import { ProductsResponse } from '../models/products-response.model';
import { StoreProductsRequest } from '../models/store-products-request.model';
import { GET_STORE_PRODUCTS_ENDPOINT } from '../shared/api-endpoints';

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
}
