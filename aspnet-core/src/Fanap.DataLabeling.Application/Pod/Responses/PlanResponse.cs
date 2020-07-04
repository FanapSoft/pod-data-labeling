namespace Fanap.DataLabeling.Clients.Pod.Responses
{
    public class PlanResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public long Price { get; set; }

        public long PeriodCount { get; set; }

        public string PeriodTypeCode { get; set; }

        public long UsageCountLimit { get; set; }

        public long UsageAmountLimit { get; set; }

        public ProductResponse[] PermittedProductList { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)} : {Id}, {nameof(Name)} : {Name}";
        }
    }
}