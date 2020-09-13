using Fanap.DataLabeling.Clients.Pod.Dtos;

namespace Fanap.DataLabeling.Clients.Pod
{
    public class PodWalletCreditResponse
    {
        public int Id { get; set; }

        public long Amount { get; set; }

        public Currency CurrencySrv { get; set; }

        public string Wallet { get; set; }

        public string WalletName { get; set; }

        public bool Active { get; set; }

        public bool Block { get; set; }

        public bool IsAutoSettle { get; set; }

        public override string ToString()
        {
            return $"{nameof(Amount)} : {Amount}, {nameof(Currency)}{nameof(Currency.Name)} : {CurrencySrv.Name}, {nameof(Active)} : {Active}";
        }
    }
}