import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CartItem } from '../shared/model/cart-item-model';
import { Cart } from '../shared/model/cart-model';
import { CartService } from '../shared/service/cart.service';
import { UserService } from '../shared/service/user.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  public cart: Cart;
  public shippingCharge = 100;
  constructor(private cartService: CartService, private router: Router, private userService: UserService) { }

  ngOnInit(): void {
    if (this.userService.isLoggedIn()) {
      this.cartService.cart.subscribe(res => this.cart = res);
    } else {
      this.cart = (JSON.parse(this.cartService.getLocalCart()) as Cart);
    }
  }

  public addQuantity(cartItem: CartItem) {
    cartItem.quantity++;
    this.cartService.updateCart(cartItem);
  }

  public removeQuantity(cartItem: CartItem) {
    cartItem.quantity--;
    this.cartService.updateCart(cartItem);
  }

  public removeCartItem(cartItem: CartItem) {
    this.cartService.removeCartItem(cartItem);
  }

  public onClickCheckout() {
    if (this.userService.isLoggedIn()) {
      this.router.navigate(['cart/address']);
    } else {
      this.router.navigate(["/login"]);
    }
  }
}
