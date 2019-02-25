import { Injectable } from "@angular/core";
import { User } from "../models/user";
import { UserService } from "../services/user.service";
import { Resolve, Router, ActivatedRouteSnapshot } from "@angular/router";
import { AlertifyService } from "../services/alertify.service";
import { Observable, of } from "rxjs";
import { catchError } from "rxjs/operators";
import { Message } from "../models/message";
import { AuthService } from "../services/auth.service";

@Injectable()
export class MessagesResolver implements Resolve<Message[]> {
  pageNumber: number = 1;
  pageSize: number = 12;
  messageContainer: string = "Unread";

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private alertify: AlertifyService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<Message[]> {
    return this.userService.getMessages(this.authService.decodedToken.nameid, this.pageNumber, this.pageSize, this.messageContainer)
      .pipe(catchError(error => {
        this.alertify.error('Problem retrieving messages');
        this.router.navigate(['/home']);
        return of(null);
      }));
  }
}
