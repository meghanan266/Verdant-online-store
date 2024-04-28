import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Product } from '../shared/model/product-model';
import { CartService } from '../shared/service/cart.service';
import { ProductsService } from './products.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {
  public productList: Product[];
  public selectedProductId: number;
  public selectedProduct: Product;

  constructor(private productService: ProductsService, private router: Router,
    private route: ActivatedRoute, private cartService: CartService) {
    this.route.paramMap.subscribe(params => {
      this.selectedProductId = +params.get('id');
    });
  }

  ngOnInit(): void {
    this.getAllProducts();
  }

  public getAllProducts() {
    this.productService.getAllProducts().subscribe(res => {
      this.productList = res;
      this.selectedProduct = this.selectedProductId != 0 ? this.productList.filter(p => p.productId == this.selectedProductId)[0] : null;
    })
  }

  public OnClickProduct(product: Product) {
    this.router.navigate(['products', product.productId, product.productName]);
  }

  public addToCart(product: Product) {
    this.cartService.addToCart(product);
  }

}
