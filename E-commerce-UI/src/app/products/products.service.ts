import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Product } from '../shared/model/product-model';
import { Review } from '../shared/model/review-model';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(private http: HttpClient) { }

  private baseUrl = `${environment.apiBaseUrl}api/product/`;

  public getAllProducts(): Observable<Product[]> {
    return this.http.get<Product[]>(this.baseUrl + "products-list");
  }

  public getProduct(id: number): Observable<Product> {
    return this.http.get<Product>(this.baseUrl + "product-item/" + id);
  }

  public getReview(productId: number) {
    return this.http.get<Review[]>(this.baseUrl + "get-review/" + productId);
  }

  public addReview(review: Review) {
    return this.http.post(this.baseUrl + "add-review", review);
  }
}
