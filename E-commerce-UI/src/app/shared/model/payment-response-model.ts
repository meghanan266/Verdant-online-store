export class PaymentResponse {
    public razorpay_payment_id: string;
    public razorpay_order_id: string;
    public razorpay_signature: string;
    public tempOrderId: string = "";
}