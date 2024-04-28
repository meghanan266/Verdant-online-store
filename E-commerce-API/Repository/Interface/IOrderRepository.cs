using E_commerce_API.DataModel;
using Razorpay.Api;

namespace E_commerce_API.Repository.Interface
{
    public interface IOrderRepository
    {
        public string CreateOrder(int totalPrice);
        public void StoreTempOrderId(string tempOrderId, int userId);
        public bool MakePayment(PaymentResponseDto paymentResponse);
        public void StoreSuccessfulOrder(SuccessfulOrderDTO successfulOrder, int userId);
        public List<SuccessfulOrderDTO> GetMyOrders(int userId);
    }
}
