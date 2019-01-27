import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';

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
      username: new FormControl('', Validators.required),
      password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
      confirmPassword: new FormControl('', Validators.required)
    }, this.passwordConfirmationValidator);
  }

  register() {
    //this.authService.register(this.model).subscribe(() => {
    //  this.alertify.success('Registered successfully')
    //}, error => {
    //  this.alertify.error(error);
    //});

    console.log(this.registerForm.value);
  }

  passwordConfirmationValidator(group: FormGroup) {
    if (group.get('password').value === group.get('confirmPassword').value) {
      return null;
    }

    return {'mismatch: true'};
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
