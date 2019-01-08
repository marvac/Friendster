import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode: boolean = false;
  values: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getValues();
  }

  getValues() {
    this.http.get('http://localhost:54314/api/values').subscribe(res => {
      this.values = res;
    }, err => {
      console.log(err);
    });
  }

  startRegisterMode() {
    this.registerMode = true;
  }

  cancelRegisterMode(event) {
    this.registerMode = event;
  }
}
