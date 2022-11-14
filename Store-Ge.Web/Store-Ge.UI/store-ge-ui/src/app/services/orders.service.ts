import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddOrderRequest } from '../models/add-order-request.model';
import { OrdersRequest } from '../models/orders-request.model';
import { OrdersResponse } from '../models/orders-response.model';
import {
  ADD_ORDER_ENDPOINT,
  GET_STORE_ORDERS_ENDPOINT,
} from '../shared/api-endpoints';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  constructor(private http: HttpClient) {}

  getStoreOrders(ordersRequest: OrdersRequest): Observable<OrdersResponse> {
    return this.http.post<OrdersResponse>(
      GET_STORE_ORDERS_ENDPOINT,
      ordersRequest
    );
  }

  addOrder(addOrderRequest: AddOrderRequest): Observable<any> {
    return this.http.post(ADD_ORDER_ENDPOINT, addOrderRequest);
  }
}
