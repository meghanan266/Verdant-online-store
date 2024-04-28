import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Address } from '../shared/model/address-model';
import { OrderResponse } from '../shared/model/order-response-model';
import { OrderService } from '../shared/service/order.service';
import { UserService } from '../shared/service/user.service';

@Component({
  selector: 'app-address',
  templateUrl: './address.component.html',
  styleUrls: ['./address.component.css']
})
export class AddressComponent implements OnInit {

  public addressList: Address[] = [];
  public addNewAddress = false;
  public editAddress = false;
  public addressForm: FormGroup = new FormGroup({
    pincode: new FormControl(null, { validators: Validators.required }),
    state: new FormControl(null, { validators: Validators.required }),
    locality: new FormControl(null, { validators: Validators.required }),
    city: new FormControl(null, { validators: Validators.required }),
    address: new FormControl(null, { validators: Validators.required }),
    addressId: new FormControl(null),
  });
  public selectedAddress: Address;

  constructor(private userService: UserService, public router: Router, private orderService: OrderService) { }

  ngOnInit(): void {
    if (this.userService.isLoggedIn()) {
      this.userService.getAddress().subscribe(res => {
        this.addressList = res;
        this.orderService.selectedAddress.next(res[0]);
      });
    }
  }

  public saveAddress() {
    this.userService.addAddress(this.addressForm.value).subscribe(res => {
      this.addressList = res;
      this.addNewAddress = false;
      this.editAddress = false;
    })
  }

  getAddressDetails() {
    this.userService.getLocation(this.addressForm.value.pincode).subscribe((response) => {
      this.addressForm.get('state').patchValue(response.state);
      this.addressForm.get('city').patchValue(response.city);
    });
  }

  proceedToPayment() {
    if (!this.userService.isLoggedIn()) {
      this.router.navigate(["/login"]);
    }
    else {
      this.orderService.onClickCheckout().subscribe((res: OrderResponse) => {
        this.orderService.storeOrderId(res.id).subscribe();
        this.orderService.makePayment(res);
      });
    }
  }

  addAddress() {
    this.addNewAddress = true;
    this.addressForm.reset();
  }

  onEditAddress(address: Address) {
    this.editAddress = true;
    this.addressForm.reset(address);
  }

  onDeleteAddress(addressId: number) {
    this.userService.onDeleteAddress(addressId).subscribe(res => {
      this.addressList = res;
    })
  }

  onChangeAddress(selectedAddress: Address) {
    this.orderService.selectedAddress.next(selectedAddress);
  }
}
