import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Address } from '../model/address-model';
import { Cart } from '../model/cart-model';
import { OrderResponse } from '../model/order-response-model';
import { SuccessfulOrder } from '../model/order-success-model';
import { PaymentResponse } from '../model/payment-response-model';
import { User } from '../model/user-model';
import { CartService } from './cart.service';
import { UserService } from './user.service';
declare let Razorpay: any;

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private baseUrl = `${environment.apiBaseUrl}api/order`;
  private user: User;
  private shippingCharge = 100;
  public selectedAddress = new BehaviorSubject<Address>(new Address());

  constructor(private http: HttpClient, private cartService: CartService, private userService: UserService, private router: Router) {
    this.userService.user.subscribe((res: User) => { this.user = res; })
  }

  public onClickCheckout(): Observable<OrderResponse> {
    const url = `${this.baseUrl}/create-order`;
    let totalPrice;
    this.cartService.cart.subscribe(res => {
      totalPrice = res.totalPrice + this.shippingCharge;
    });
    const options = {
      params: new HttpParams().set('totalPrice', totalPrice)
    };
    return this.http.get<OrderResponse>(url, options);
  }

  public storeOrderId(orderId: string) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = { headers };
    const url = `${this.baseUrl}/store-order-id`;
    return this.http.post<void>(url, JSON.stringify(orderId), options);
  }

  public makePayment(res: OrderResponse) {
    const options = {
      "key": "rzp_test_anzIjjz2Vd1MvW", // Enter the Key ID generated from the Dashboard
      "amount": res.amount, // Amount is in currency subunits. Default currency is INR. Hence, 50000 refers to 50000 paise
      "currency": "INR",
      "name": "Megh Corp", //your business name
      "description": "Test Transaction",
      "image": "/src/assets/images/logo.png",
      "order_id": res.id, //This is a sample Order ID. Pass the `id` obtained in the response of Step 1
      "handler": (response: PaymentResponse) => {
        response.tempOrderId = res.id;
        this.handlePaymentResponse(response);
      },
      "prefill": {
        "name": this.user.userName, //your customer's name
        "email": this.user.email,
        "contact": this.user.phone
      },
      "theme": {
        "color": "#3399cc"
      }
    };
    const rzp1 = new Razorpay(options);
    rzp1.open();
  }

  private handlePaymentResponse(res: any) {
    const url = `${this.baseUrl}/make-payment`;
    this.http.post(url, res).subscribe(result => {
      if (result) {
        this.storeSuccessfulOrder(res.razorpay_order_id);
      }
    });
  }

  private storeSuccessfulOrder(razorpay_order_id: string) {
    const url = `${this.baseUrl}/store-success-order`;
    let successfulOrder = new SuccessfulOrder();
    successfulOrder.razorPayOrderId = razorpay_order_id;
    successfulOrder.productList = this.cartService.cart.value.cartItems.map(c => c.product);
    successfulOrder.deliveryAddress = this.selectedAddress.value.address + "," + this.selectedAddress.value.locality + "," +
      this.selectedAddress.value.city + "," + this.selectedAddress.value.state + "-" + this.selectedAddress.value.pincode;
    this.http.post(url, successfulOrder).subscribe(() => {
      this.cartService.cart.next(new Cart);
      this.cartService.emptyCart().subscribe(() => location.reload());
      this.router.navigate(['orders/success']);
    });
  }

  public getMyOrders() {
    const url = `${this.baseUrl}/get-my-orders`;
    return this.http.get<SuccessfulOrder[]>(url);
  }
}
