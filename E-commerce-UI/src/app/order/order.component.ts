import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SuccessfulOrder } from '../shared/model/order-success-model';
import { OrderService } from '../shared/service/order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  public status: string;
  public myOrderList: SuccessfulOrder[];
  constructor(private route: ActivatedRoute, private orderService: OrderService) {
    this.status = route.snapshot.paramMap.get('status');
  }

  ngOnInit(): void {
    this.orderService.getMyOrders().subscribe((res: SuccessfulOrder[]) => {
      this.myOrderList = res;
    });
  }

}
