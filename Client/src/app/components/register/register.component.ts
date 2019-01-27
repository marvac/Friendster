import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { FormGroup, FormControl } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();

  model: any = {};
  registerForm: FormGroup;

  constructor(private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.registerForm = new FormGroup({
      username: new FormControl(),
      password: new FormControl(),
      confirmPassword: new FormControl();
    });
  }

  register() {
    //this.authService.register(this.model).subscribe(() => {
    //  this.alertify.success('Registered successfully')
    //}, error => {
    //  this.alertify.error(error);
    //});

    console.log(this.registerForm.value);
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
