import { Component, OnInit } from '@angular/core';
import { ProductsService } from '../products/products.service';
import { Product, RowStatus } from '../shared/model/product-model';
import { FormArray, FormBuilder, FormGroup } from '@angular/forms';
import { Alert } from '../shared/model/alert-model';

@Component({
  selector: 'app-dashboard-product',
  templateUrl: './dashboard-product.component.html',
  styleUrls: ['./dashboard-product.component.css']
})
export class DashboardProductComponent implements OnInit {
  productList: Product[];
  productListCopy: Product[];
  productForm: FormGroup;
  isEditEnabled = false;
  public alert: Alert;

  constructor(private productService: ProductsService, private fb: FormBuilder) {
    this.productForm = this.fb.group({
      products: this.fb.array([])
    });
  }

  ngOnInit(): void {
    this.productService.getAllProducts().subscribe(res => {
      this.productList = res;
      this.productListCopy = JSON.parse(JSON.stringify(this.productList));
    });
  }

  onClickAdd() {
    this.productList.unshift({
      rowStatus: RowStatus.NEW,
      productName: '',
      productId: 0,
      productDescription: '',
      price: null,
      productQuantity: '',
      pictureUrl: []
    } as Product);
  }

  onClickCancel() {
    this.isEditEnabled = false;
    this.productList = JSON.parse(JSON.stringify(this.productListCopy));
  }

  deleteRow(productId: number) {
    this.productList.find(p => p.productId === productId).rowStatus = RowStatus.DELETED;
  }

  save() {
    if (this.productList.every(p =>
      p.price != null && p.productDescription != '' && p.productName != '' && p.productQuantity != ''
    )) {
      this.productService.saveProduct(this.productList).subscribe(res => {
        this.productList = res;
        this.productListCopy = JSON.parse(JSON.stringify(this.productList));
        this.isEditEnabled = false;
      });
    } else {
      this.alert = {
        show: true,
        type: 'fail',
        message: 'Please fill all fields.'
      };
    }
  }

}
