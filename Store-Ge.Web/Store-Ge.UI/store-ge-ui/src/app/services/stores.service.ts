import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Observable } from 'rxjs';
import { Store } from '../models/store.model';
import { GET_USER_STORES_ENDPOINT } from '../shared/api-endpoints';

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
}
