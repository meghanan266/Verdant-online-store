import { Product } from "./product-model";

export class SuccessfulOrder {
    public successfulOrderId: number;
    public razorPayOrderId: string;
    public productList: Product[];
    public deliveryAddress: string;
    public orderDate: Date;
}