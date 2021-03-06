import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable, BehaviorSubject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt'
import { User } from '../models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private baseApiUrl = environment.baseApiUrl;
  private loginUrl = `${this.baseApiUrl}/auth/login`;
  private registerUrl = `${this.baseApiUrl}/auth/register`;
  private jwtHelper = new JwtHelperService();

  public currentUser: User;
  public decodedToken: any;
  public photoUrl = new BehaviorSubject<string>('../../assets/placeholder.png');
  public currentPhotoUrl = this.photoUrl.asObservable();

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
            this.changeProfilePhoto(this.currentUser.photoUrl);
          }
        })
      );
  }

  register(user: User) {
    return this.http.post(this.registerUrl, user);
  }

  isAuthenticated(): boolean {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  changeProfilePhoto(photoUrl: string) {
    this.photoUrl.next(photoUrl);
  }
}
