import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user';
import { PaginatedResult } from '../models/pagination';
import { map } from 'rxjs/operators';
import { Message } from '../models/message';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  private usersUrl = '/api/users/';

  constructor(private http: HttpClient) { }

  getUsers(page?, itemsPerPage?, userParameters?, likesParameter?): Observable<PaginatedResult<User[]>> {

    const paginatedResult: PaginatedResult<User[]> = new PaginatedResult<User[]>();

    let params = new HttpParams();

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (userParameters != null) {
      params = params.append('minimumAge', userParameters.minAge);
      params = params.append('maximumAge', userParameters.maxAge);
      params = params.append('gender', userParameters.gender);
      params = params.append('orderBy', userParameters.orderBy);
    }

    if (likesParameter != null) {
      params = params.append(likesParameter, 'true');
    }

    return this.http.get<User[]>(this.usersUrl, { observe: 'response', params })
      .pipe(map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }

        return paginatedResult;
      }));
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

  likeUser(userId: number, recipientId: number) {
    return this.http.post(`${this.usersUrl}${userId}/like/${recipientId}`, {});
  }

  getMessages(userId: number, page?, itemsPerPage?, messageContainer?): Observable<PaginatedResult<Message[]>> {
    const paginatedResult: PaginatedResult<Message[]> = new PaginatedResult<Message[]>();
    let params = new HttpParams();
    params = params.append('MessageContainer', messageContainer);

    if (page != null && itemsPerPage != null) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    return this.http.get<Message[]>(`${this.usersUrl}${userId}/messages`, { observe: 'response', params })
      .pipe(map(response => {
        paginatedResult.result = response.body;
        if (response.headers.get('Pagination') != null) {
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }

        return paginatedResult;
      }));
  }

  getMessageThread(userId: number, recipientId: number) {
    return this.http.get<Message[]>(`${this.usersUrl}${userId}/messages/thread/${recipientId}`);
  }

  sendMessage(userId: number, message: Message) {
    return this.http.post(`${this.usersUrl}${userId}/messages/`, message);
  }

  deleteMessage(userId: number, messageId: number) {
    return this.http.post(`${this.usersUrl}${userId}/messages/${messageId}`, {});
  }

  markAsRead(userId: number, messageId: number) {
    //subscribe here since we don't need to do anything with the returned info
    return this.http.post(`${this.usersUrl}${userId}/messages/${messageId}/read`, {})
      .subscribe();
  }
}
