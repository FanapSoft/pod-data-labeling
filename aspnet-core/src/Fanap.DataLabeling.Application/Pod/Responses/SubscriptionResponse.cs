using Fanap.DataLabeling.Clients.Pod.Dtos;

namespace Fanap.DataLabeling.Clients.Pod.Responses
{
    public class SubscriptionResponse
    {
        public int Id { get; set; }

        public long FromDate { get; set; }

        public long ToDate { get; set; }

        public long CreationDate { get; set; }

        public long UsageCount { get; set; }

        public long UsedAmount { get; set; }

        public string Status { get; set; }

        public PlanResponse Plan { get; set; }

        public Currency Currency { get; set; }

        public int Count { get; set; }

        public Invoice InvoiceSrv { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)} : {Id}, {nameof(Status)} : {Status}," +
                   $" {nameof(Plan)} : '{Plan}'" +
                   $" {nameof(Currency)} : '{Currency}'";
        }
    }
}