import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { SuccessfulOrder } from '../model/order-success-model';
import { Observable } from 'rxjs';
import { User } from '../model/user-model';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  private baseUrl = `${environment.apiBaseUrl}api`;

  constructor(private http: HttpClient) { }

  public getAllOrders(value: string): Observable<SuccessfulOrder[]> {
    const options = {
      params: new HttpParams().set('filterValue', value)
    };
    return this.http.get<SuccessfulOrder[]>(this.baseUrl + '/order/get-all-orders', options);
  }

  public saveDashboardOrder(modifiedOrders: SuccessfulOrder[]): Observable<SuccessfulOrder[]> {
    return this.http.post<SuccessfulOrder[]>(this.baseUrl + '/order/save-dashboard-order', modifiedOrders);
  }

  public getAllUsers() {
    return this.http.get<User[]>(this.baseUrl + '/user/get-all-users');
  }
}
