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

  setMainPhoto(userId: number, photoId: number) {
    return this.http.post(`${this.usersUrl}${userId}/photos/${photoId}/setMain`, {});
  }

  deletePhoto(userId: number, photoId: number) {
    return this.http.delete(`${this.usersUrl}${userId}/photos/${photoId}`);
  }
}
