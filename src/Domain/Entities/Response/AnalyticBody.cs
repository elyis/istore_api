namespace istore_api.src.Domain.Entities.Response
{
    public class AnalyticBody
    {
        public int CountOrders { get; set; } = 0;
        public float AverageSum { get; set; } = 0;
        public int CountPurchasedGoods { get; set; } = 0;
        public float MinCostProduct { get; set; } = 0;
        public float MaxCostProduct { get; set; } = 0;
        public List<ProductAnalyticBody> ProductAnalytics { get; set; } = new();
    }
}