import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router } from '@angular/router';
import { UserService } from '../shared/service/user.service';

@Injectable({
  providedIn: 'root'
})
export class RedirectGuard implements CanActivate {

  constructor(private userService: UserService, private router: Router) { }

  canActivate(): boolean {
    const userRole = this.userService.getUserRole();

    if (userRole === 'Admin') {
      this.router.navigate(['dashboard']);
    } else {
      this.router.navigate(['home']);
    }
    return false;
  }
}

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private userService: UserService, private router: Router) { }

  canActivate(next: ActivatedRouteSnapshot): boolean {
    const expectedRole = next.data['role'];
    if (this.userService.hasRole(expectedRole)) {
      return true;
    } else {
      this.router.navigate(['login']);
      return false;
    }
  }
}
