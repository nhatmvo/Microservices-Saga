namespace PaymentApi.Models.ApiModels
{
    public class TopUpBalanceRequest
    {
        public int BudgetId { get; set; }

        public decimal Balance { get; set; }
    }
}
