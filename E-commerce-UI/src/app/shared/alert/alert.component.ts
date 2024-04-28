import { Component, Input, OnInit } from '@angular/core';
import { Alert } from '../model/alert-model';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent implements OnInit {

  @Input() public alert: Alert;
  constructor() { }

  ngOnInit(): void {

  }

  public resetAlert() {
    this.alert = {
      show: false,
      type: '',
      message: ''
    }
  }
}
