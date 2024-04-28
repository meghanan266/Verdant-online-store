import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { catchError, Observable, switchMap, throwError } from 'rxjs';
import { UserService } from '../service/user.service';
import { Token } from '../model/token-model';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private userService: UserService) { }

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const token = this.userService.getToken();
    if (token) {
      request = request.clone({
        setHeaders: { Authorization: `Bearer ${token}` }
      })
    }

    return next.handle(request).pipe(catchError(err => {
      if (err instanceof HttpErrorResponse) {
        return this.handleUnauthorizedError(request, next);
      }

      return throwError(() => new Error("Something went wrong"));
    }));
  }

  handleUnauthorizedError(req: HttpRequest<any>, next: HttpHandler) {
    const token = new Token();
    token.accessToken = this.userService.getToken();
    token.refreshToken = this.userService.getRefreshToken();

    return this.userService.renewToken(token).pipe(
      switchMap((data: Token) => {
        this.userService.storeRefreshToken(data.refreshToken);
        this.userService.storeToken(data.accessToken);
        req = req.clone({
          setHeaders: { Authorization: `Bearer ${data.accessToken}` }
        })
        return next.handle(req);
      }),
      catchError((err) => {
        return throwError(() => {
          console.warn("Token is expired");
          this.userService.logOut();
          // location.reload();
        })
      })
    );
  }
}
