import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ResetPassword } from '../model/reset-password-model';

@Injectable({
  providedIn: 'root'
})
export class ResetPasswordService {

  private baseUrl = `${environment.apiBaseUrl}api/user`;
  constructor(private http: HttpClient) { }

  public sendResetPasswordLink(email: string) {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const options = { headers };
    return this.http.post<void>(`${this.baseUrl}/send-reset-email`, JSON.stringify(email), options);

  }

  public resetPassword(resetPassword: ResetPassword) {
    return this.http.post(`${this.baseUrl}/reset-password`, resetPassword);
  }
}
