import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductsService } from '../products/products.service';
import { Alert } from '../shared/model/alert-model';
import { Cart } from '../shared/model/cart-model';
import { Product } from '../shared/model/product-model';
import { Review } from '../shared/model/review-model';
import { CartService } from '../shared/service/cart.service';
import { UserService } from '../shared/service/user.service';

@Component({
  selector: 'app-product-item',
  templateUrl: './product-item.component.html',
  styleUrls: ['./product-item.component.css']
})
export class ProductItemComponent implements OnInit {

  public selectedProductId: number;
  public selectedProduct: Product;
  public cart: Cart;
  public reviewList: Review[];
  public reviewRange = 100;
  public isAddReview = false;
  public newReview = new Review;
  public isUserLoggedIn = false;
  public alert: Alert;
  public avgReview: number;

  constructor(private productService: ProductsService,
    private route: ActivatedRoute, private cartService: CartService, private userService: UserService) {
    this.route.paramMap.subscribe(params => {
      this.selectedProductId = +params.get('id');
    });
    if (userService.isLoggedIn()) {
      this.isUserLoggedIn = true;
    }
  }

  ngOnInit(): void {
    this.loadData();
  }

  loadData() {
    this.productService.getProduct(this.selectedProductId).subscribe((res: Product) => {
      this.selectedProduct = res;
    });
    this.productService.getReview(this.selectedProductId).subscribe((res: Review[]) => {
      this.reviewList = res;
      this.avgReview = Math.round((this.reviewList.map(r => r.reviewRange).reduce( ( p, c ) => p + c, 0 ) / this.reviewList.length) / 20);
    });
  }

  public addToCart() {
    this.cartService.addToCart(this.selectedProduct);
  }

  onReviewRangeChange(event: any) {
    this.reviewRange = Number(event.target.value);
  }

  onReviewDescChange(event: any) {
    this.newReview.reviewDescription = event.target.value;
  }

  onClickSubmit() {
    this.newReview.productId = this.selectedProductId;
    this.newReview.reviewRange = this.reviewRange;
    this.productService.addReview(this.newReview).subscribe((res: any) => {
      if (res == 200) {
        this.alert = {
          show: true,
          type: 'success',
          message: "Review added successfully!"
        };
        this.loadData();
      }
      else {
        this.alert = {
          show: true,
          type: 'fail',
          message: "We value customer feedback and kindly request that reviews be submitted by those who have purchased and experienced the product. Thank you for your understanding."
        };
      }
      this.isAddReview = false;
    });
  }
}
