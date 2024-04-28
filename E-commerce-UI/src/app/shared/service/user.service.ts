import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { environment } from 'src/environments/environment';
import { Address } from '../model/address-model';
import { Token } from '../model/token-model';
import { User } from '../model/user-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl = `${environment.apiBaseUrl}api/user`;
  public user = new BehaviorSubject<User>(new User());

  constructor(private http: HttpClient) {
    if (this.getToken() != null)
      this.decodeToken(this.getToken());
  }

  public loginUser(user: User): Observable<Token> {
    const url = `${this.baseUrl}/authenticate`;
    return this.http.post<Token>(url, user);
  }

  public addUser(user: User): Observable<User> {
    const url = `${this.baseUrl}/add-user`;
    return this.http.post<User>(url, user);
  }

  public storeToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue);
    this.decodeToken(tokenValue);
  }

  public getToken() {
    return localStorage.getItem('token');
  }

  public storeRefreshToken(tokenValue: string) {
    localStorage.setItem('refreshToken', tokenValue);
  }

  public getRefreshToken() {
    return localStorage.getItem('refreshToken');
  }

  public isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  public logOut() {
    localStorage.clear();
  }

  public decodeToken(token: string) {
    const jwtHelper = new JwtHelperService();
    const decodedToken = jwtHelper.decodeToken(token);
    const userInfo = new User();
    userInfo.role = decodedToken.role;
    userInfo.userName = decodedToken.unique_name;
    userInfo.email = decodedToken.email;
    userInfo.phone = decodedToken.phone;
    this.user.next(userInfo);
  }

  public updateUser(user: User): Observable<User> {
    const url = `${this.baseUrl}/update-user`;
    return this.http.post<User>(url, user);
  }

  public renewToken(token: Token) {
    const url = `${this.baseUrl}/refresh-token`;
    return this.http.post<Token>(url, token);
  }

  public getAddress(): Observable<Address[]> {
    const url = `${this.baseUrl}/get-address`;
    return this.http.get<Address[]>(url);
  }

  public addAddress(address: Address) {
    const url = `${this.baseUrl}/add-address`;
    return this.http.post<Address[]>(url, address);
  }

  public getLocation(pincode: number) {
    const url = `${this.baseUrl}/get-location/${pincode}`;
    return this.http.get<Address>(url);
  }

  public onDeleteAddress(addressId: number) {
    const url = `${this.baseUrl}/delete-address/${addressId}`;
    return this.http.delete<Address[]>(url);
  }
}
