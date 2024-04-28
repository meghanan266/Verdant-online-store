namespace E_commerce_API.DataModel
{
    public class PaymentResponseDto
    {
        public string Razorpay_payment_id { get; set; }
        public string Razorpay_order_id { get; set; }
        public string Razorpay_signature { get; set; }
        public string TempOrderId { get; set; }
    }
}
