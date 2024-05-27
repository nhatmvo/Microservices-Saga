using PaymentApi.Models.ApiModels;

namespace PaymentApi.Services
{
    public interface IPaymentService
    {
        public TopUpBalanceResponse TopUpBalance(TopUpBalanceRequest request);

        public void FulfillOrderPayment(int budgetId, decimal price, int orderId);
    }
}
