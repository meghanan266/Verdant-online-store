import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Alert } from '../shared/model/alert-model';
import { Token } from '../shared/model/token-model';
import { User } from '../shared/model/user-model';
import { CartService } from '../shared/service/cart.service';
import { ResetPasswordService } from '../shared/service/reset-password.service';
import { UserService } from '../shared/service/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public isSignUp = false;
  public loginForm: FormGroup = new FormGroup({
    email: new FormControl(null, { validators: Validators.required }),
    password: new FormControl(null, { validators: Validators.required })
  });

  public signUpForm: FormGroup = new FormGroup({
    userName: new FormControl(null, { validators: Validators.required }),
    email: new FormControl(null, { validators: Validators.required }),
    password: new FormControl(null, { validators: Validators.required }),
    phone: new FormControl(null, { validators: Validators.required }),
  });

  public user: User = new User();
  public isLogInFail = false;
  public alert: Alert;
  public forgotPwdEmail: string;
  constructor(private userService: UserService, private router: Router, private cartService: CartService, private resetPwdService: ResetPasswordService) { }

  ngOnInit(): void {
    if (this.userService.isLoggedIn()) {
      this.router.navigate(['profile']);
    }
  }

  onClickLogin() {
    this.userService.loginUser(this.loginForm.value).subscribe((res: Token) => {
      if (res == null) {
        this.isLogInFail = true;
        this.alert = {
          show: true,
          type: 'fail',
          message: 'Login Failed :/'
        };
        return;
      }
      this.alert = {
        show: true,
        type: 'success',
        message: 'Logged In Successfully!'
      };
      this.userService.storeToken(res.accessToken);
      this.userService.storeRefreshToken(res.refreshToken);
      this.userService.user.subscribe(u => { this.user = u; })
      this.loginForm.reset();
      if (this.userService.isLoggedIn())
        this.cartService.getAllCartItems().subscribe(c => { this.cartService.cart.next(c) });
      setTimeout(() => {
        this.router.navigate(['']).then(() => {
          window.location.reload();
        });
      }, 3000);
    });
  }

  onClickSignUp() {
    this.userService.addUser(this.signUpForm.value).subscribe((res: User) => {
      if (res.token == null) {
        this.isLogInFail = true;
        this.alert = {
          show: true,
          type: 'fail',
          message: 'User already exists. Please login.'
        };
        return;
      }
      this.alert = {
        show: true,
        type: 'success',
        message: 'Account created successfully! Please login.'
      };
      this.user = res;
      this.signUpForm.reset();
      this.isSignUp = false;
      this.userService.storeToken(res.token);
      setTimeout(() => { this.router.navigate(['']) }, 3000);
    });
  }

  resetPassword() {
    this.resetPwdService.sendResetPasswordLink(this.forgotPwdEmail).subscribe({
      next: (res) => {
        this.forgotPwdEmail = "";
        const button = document.getElementById("closeModal");
        button?.click();
        this.alert = {
          show: true,
          type: 'success',
          message: 'Email to reset password has been sent successfully.'
        };
      },
      error: (err) => {
        this.alert = {
          show: true,
          type: 'fail',
          message: 'Something went wrong.'
        };
      }
    })
  }

}
