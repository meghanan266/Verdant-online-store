import { Component, OnInit } from '@angular/core';
import { Alert } from '../shared/model/alert-model';
import { ProductsService } from '../products/products.service';
import { Product } from '../shared/model/product-model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  public alert: Alert;
  public bestSellerList: Product[];
  constructor(private productService: ProductsService, private router: Router) { }

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe(res => {
      this.bestSellerList = res.slice(0, 3);
    });
  }

  public OnClickProduct(product: Product) {
    this.router.navigate(['products', product.productId, product.productName]);
  }
}
