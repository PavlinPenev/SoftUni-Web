import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Supplier } from '../models/supplier.model';
import { GET_USER_SUPPLIERS_ENDPOINT } from '../shared/api-endpoints';

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
}
