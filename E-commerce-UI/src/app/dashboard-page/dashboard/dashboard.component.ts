import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { DashboardService } from '../../shared/service/dashboard.service';
import { SuccessfulOrder } from '../../shared/model/order-success-model';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DashboardComponent implements OnInit {

  public orders: SuccessfulOrder[];
  public ordersCopy: SuccessfulOrder[];
  isEditEnabled = false;
  totalIncome: number;
  totalUsers: number;
  selectedFilterValue: string = '';

  constructor(private dashboardService: DashboardService) {
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.dashboardService.getAllOrders(this.selectedFilterValue).subscribe(res => {
      this.orders = res;
      this.ordersCopy = JSON.parse(JSON.stringify(this.orders));
      this.totalIncome = this.orders.reduce((total, order) => {
        return total + order.productPrice;
      }, 0);
      this.totalUsers = this.orders.map(o => o.userId).filter((value, index, self) => self.indexOf(value) === index).length;
    });
  }

  onClickCancel() {
    this.isEditEnabled = false;
    this.orders = JSON.parse(JSON.stringify(this.ordersCopy));
  }

  save() {
    const modifiedOrders = this.orders.filter(ord =>
      this.ordersCopy.find(o => o.customOrderId === ord.customOrderId).deliveryStatus !== ord.deliveryStatus ||
      this.ordersCopy.find(o => o.customOrderId === ord.customOrderId).deliveryTrackingId !== ord.deliveryTrackingId
    );
    if (modifiedOrders.length > 0) {
      this.dashboardService.saveDashboardOrder(modifiedOrders).subscribe(res => {
        this.orders = res;
        this.ordersCopy = JSON.parse(JSON.stringify(this.orders));
        this.isEditEnabled = false;
      });
    }
  }

  onSelectionChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.selectedFilterValue = target.value;
    this.loadData();
  }
}
