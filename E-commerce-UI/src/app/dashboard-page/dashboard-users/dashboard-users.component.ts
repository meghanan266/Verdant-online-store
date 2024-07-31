import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/shared/model/user-model';
import { DashboardService } from 'src/app/shared/service/dashboard.service';

@Component({
  selector: 'app-dashboard-users',
  templateUrl: './dashboard-users.component.html',
  styleUrls: ['./dashboard-users.component.css']
})
export class DashboardUsersComponent implements OnInit {
  usersList: User[];

  constructor(private dashboardService: DashboardService) { }

  ngOnInit(): void {
    this.dashboardService.getAllUsers().subscribe(res => {
      this.usersList = res;
    });
  }

}
