import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private loginUrl = '/api/auth/login';
  private registerUrl = '/api/auth/register';

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post(this.loginUrl, model)
      .pipe(
        map((res: any) => {
          const user = res;
          if (user) {
            localStorage.setItem('token', user.token);
          }
        })
      );
  }

  register(model: any) {
    return this.http.post(this.registerUrl, model);
  }
}
