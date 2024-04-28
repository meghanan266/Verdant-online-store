import { CartItem } from "./cart-item-model";

export class Cart {
    public cartId: number;
    public cartItems: CartItem[] = [];
    public userId: number;
    public totalPrice: number = 0;
}