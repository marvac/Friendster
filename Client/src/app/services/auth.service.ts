import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt'
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loginUrl = '/api/auth/login';
  private registerUrl = '/api/auth/register';
  private jwtHelper = new JwtHelperService();

  public currentUser: User;
  public decodedToken: any;

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.loginUrl, model)
      .pipe(
        map((res: any) => {
          const user = res;
          if (user) {
            localStorage.setItem('token', user.token);
            localStorage.setItem('user', JSON.stringify(user.userResource));
            this.decodedToken = this.jwtHelper.decodeToken(user.token);
            this.currentUser = user.userResource;
          }
        })
      );
  }

  register(model: any) {
    return this.http.post(this.registerUrl, model);
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }
}
