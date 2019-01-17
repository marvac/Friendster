import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersUrl = '/api/users/';

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.usersUrl);
  }

  getUser(id): Observable<User> {
    return this.http.get<User>(`${this.usersUrl}${id}`);
  }

  updateUser(id: number, user: User) {
    return this.http.put(`${this.usersUrl}${id}`, user);
  }
}
