import { Component, Input, OnInit } from '@angular/core';
import { Alert } from '../model/alert-model';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent {

  @Input() public alert: Alert;
  constructor() { }

  public resetAlert() {
    this.alert = {
      show: false,
      type: '',
      message: ''
    }
  }
}
