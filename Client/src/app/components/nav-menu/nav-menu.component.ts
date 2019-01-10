import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { AlertifyService } from 'src/app/services/alertify.service';

@Component({
  selector: 'nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() { }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success(`Welcome back, ${this.model.username}`);
    }, error => {
      this.alertify.error(error);
      });
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('Logged out');
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();
  }
}
