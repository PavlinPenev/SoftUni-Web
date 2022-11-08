import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AddStoreRequest } from '../models/add-store-request.model';
import { Store } from '../models/store.model';
import {
  ADD_STORE_ENDPOINT,
  GET_STORE_ENDPOINT,
  GET_USER_STORES_ENDPOINT,
} from '../shared/api-endpoints';

@Injectable({
  providedIn: 'root',
})
export class StoresService {
  constructor(private http: HttpClient) {}

  getUserStores(userId: string): Observable<Store[]> {
    return this.http.get<Store[]>(GET_USER_STORES_ENDPOINT, {
      params: {
        userId,
      },
    });
  }

  getStore(storeId: string): Observable<Store> {
    return this.http.get<Store>(GET_STORE_ENDPOINT, {
      params: {
        storeId,
      },
    });
  }

  addStore(request: AddStoreRequest): Observable<any> {
    return this.http.post(ADD_STORE_ENDPOINT, request);
  }
}
