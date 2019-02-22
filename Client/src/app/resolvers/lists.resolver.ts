import { Injectable } from "@angular/core";
import { User } from "../models/user";
import { UserService } from "../services/user.service";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { AlertifyService } from "../services/alertify.service";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable()
export class ListsResolver implements Resolve<User[]> {
  pageNumber: number = 1;
  pageSize: number = 12;
  likesParameter = 'likers';

  constructor(
    private userService: UserService,
    private router: Router,
    private alertify: AlertifyService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<User[]> {
    return this.userService.getUsers(this.pageNumber, this.pageSize, null, this.likesParameter)
      .pipe(catchError(error => {
        this.alertify.error('Problem retrieving data');
        this.router.navigate(['/home']);
        return of(null);
      }));
  }
}
