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
  public reviews: { url: string, type: 'image' | 'video' }[] = [];

  constructor(private productService: ProductsService, private router: Router) { }

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe(res => {
      this.bestSellerList = res.slice(0, 3);
    });
    this.reviews = [
      { url: 'assets/images/Reviews/review-1.mp4', type: 'video' },
      { url: 'assets/images/Reviews/review-2.jpg', type: 'image' },
      { url: 'assets/images/Reviews/review-3.mp4', type: 'video' },
      { url: 'assets/images/Reviews/review-4.mp4', type: 'video' },
      { url: 'assets/images/Reviews/review-5.jpg', type: 'image' },
      { url: 'assets/images/Reviews/review-6.mp4', type: 'video' },
      { url: 'assets/images/Reviews/review-7.jpg', type: 'image' },
      { url: 'assets/images/Reviews/review-8.jpg', type: 'image' },
      { url: 'assets/images/Reviews/review-9.jpg', type: 'image' },
      { url: 'assets/images/Reviews/review-10.jpg', type: 'image' },
      { url: 'assets/images/Reviews/review-11.jpg', type: 'image' },
      { url: 'assets/images/Reviews/review-12.jpg', type: 'image' },
    ];
  }

  public OnClickProduct(product: Product) {
    this.router.navigate(['products', product.productId, product.productName]);
  }
}
