import { Component, OnInit } from '@angular/core';
import { CartService } from '../shared/service/cart.service';
import { UserService } from '../shared/service/user.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.css']
})
export class NavBarComponent implements OnInit {

  public menuIconVisible = false;
  public isLoggedIn = false;
  public userName: string;
  public totalCartItems: number;

  constructor(private userService: UserService, private cartService: CartService) { }

  ngOnInit(): void {
    this.isLoggedIn = this.userService.isLoggedIn();
    this.cartService.cart.subscribe(c => {
      this.totalCartItems = 0;
      c.cartItems.forEach(i => {
        this.totalCartItems += i.quantity
      })
    });

    if (this.isLoggedIn)
      this.userService.user.subscribe(u => { this.userName = u.userName; })
  }

  public openClickMenuIcon() {
    this.menuIconVisible = !this.menuIconVisible;
  }

  public onClickLogOut() {
    this.userService.logOut();
    location.reload();
  }
}
