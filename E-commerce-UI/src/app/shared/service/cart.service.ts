import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { CartItem } from '../model/cart-item-model';
import { Cart } from '../model/cart-model';
import { Product } from '../model/product-model';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class CartService {

  public cart = new BehaviorSubject<Cart>(new Cart());
  private baseUrl = `${environment.apiBaseUrl}api/cart`;

  constructor(private userService: UserService, private router: Router, private http: HttpClient) {
    if (JSON.parse(this.getLocalCart()) as Cart != null)
      this.cart.next(JSON.parse(this.getLocalCart()) as Cart);
    if (this.userService.isLoggedIn() && this.cart.value.cartItems.length == 0)
      this.getAllCartItems().subscribe(c => { this.cart.next(c) });
  }

  public addToCart(selectedProduct: Product) {
    const cartTemp = this.cart.value;
    if (cartTemp.cartItems.some(c => c.product.productId == selectedProduct.productId)) {
      this.updateCart(<CartItem>{
        product: selectedProduct,
        quantity: ++cartTemp.cartItems.find(c => c.product.productId == selectedProduct.productId).quantity
      });
    }
    else {
      this.updateCart(<CartItem>{ product: selectedProduct, quantity: 1 });
    }
  }

  public updateCart(cartItem: CartItem) {
    const cartTemp = new Cart();
    const cartItemList = this.cart.value.cartItems;
    if (cartItemList.find(c => c.product.productId == cartItem.product.productId)) {
      cartItemList.find(c => c.product.productId == cartItem.product.productId).quantity = cartItem.quantity;
    }
    else {
      cartItemList.push(cartItem);
    }
    cartTemp.cartItems = cartItemList;
    cartTemp.totalPrice = 0;
    cartTemp.cartItems.forEach(c => {
      cartTemp.totalPrice += c.product.price * c.quantity;
    });
    this.cart.next(cartTemp);
    if (!this.userService.isLoggedIn()) {
      this.setLocalCart(this.cart.value);
    }
    else {
      const url = `${this.baseUrl}/addToCart`;
      this.http.post<Cart>(url, cartItem).subscribe()
    }
  }

  public getLocalCart() {
    return localStorage.getItem("localcart");
  }

  public setLocalCart(cart: Cart) {
    localStorage.setItem("localcart", JSON.stringify(cart));;
  }

  public removeCartItem(cartItem: CartItem) {
    const cartTemp = this.cart.value;
    cartTemp.totalPrice = 0;
    cartTemp.cartItems.splice(cartTemp.cartItems.findIndex(c => c.product.productId == cartItem.product.productId), 1);
    cartTemp.cartItems.forEach(c => {
      cartTemp.totalPrice += c.product.price * c.quantity;
    });
    this.cart.next(cartTemp);
    if (!this.userService.isLoggedIn()) {
      this.setLocalCart(this.cart.value);
    }
    else {
      const url = `${this.baseUrl}/removeFromCart`;
      this.http.post<Cart>(url, cartItem).subscribe();
    }
  }

  public getAllCartItems(): Observable<Cart> {
    const url = `${this.baseUrl}/getAllCartItems`;
    return this.http.get<Cart>(url);
  }

  public emptyCart() {
    const url = `${this.baseUrl}/empty-cart`;
    return this.http.delete(url);
  }
}
