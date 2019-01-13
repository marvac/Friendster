import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { Router } from '@angular/router';

@Component({
  selector: 'nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {
  model: any = {};

  constructor(public authService: AuthService, private alertify: AlertifyService, private router: Router) { }

  ngOnInit() { }

  login() {
    this.authService.login(this.model).subscribe(next => {
      this.alertify.success(`Welcome back, ${this.model.username}`);
      this.router.navigate(['/members']);
    }, error => {
      this.alertify.error(error);
      });
  }

  logout() {
    localStorage.removeItem('token');
    this.alertify.message('Logged out');
    this.router.navigate(['/home']);
  }

  isAuthenticated(): boolean {
    return this.authService.isAuthenticated();

  }
}
