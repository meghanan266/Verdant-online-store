import { CartItem } from "./cart-item-model";
import { DeliveryTracking } from "./delivery-tracking";

export class SuccessfulOrder {
    public successfulOrderId: number;
    public razorPayOrderId: string;
    public productList: CartItem[];
    public deliveryAddress: string;
    public orderDate: Date;
    public userId: number;
    public deliveryTrackingId: string;
    public deliveryStatus: boolean;
    public product: CartItem;
    public deliveryTracking: DeliveryTracking;
    public productPrice: number;
    public customOrderId: string;
}