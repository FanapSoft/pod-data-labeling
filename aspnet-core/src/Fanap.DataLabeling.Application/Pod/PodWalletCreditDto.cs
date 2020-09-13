namespace Fanap.DataLabeling.Clients.Pod
{
    public class PodWalletCreditDto
    {
        public PodWalletCreditDto()
        {

        }

        public PodWalletCreditDto(PodWalletCreditResponse response)
        {
            if (response == null)
                return;

            Amount = response.Amount;
            Wallet = response.Wallet;
            WalletName = response.WalletName;
            IsActive = response.Active;
            IsBlock = response.Block;
            IsAutoSettle = response.IsAutoSettle;

            if (response.CurrencySrv == null)
                return;

            CurrencyCode = response.CurrencySrv.Code;
            CurrencyName = response.CurrencySrv.Name;
        }

        public long Amount { get; set; }

        public string CurrencyCode { get; set; }

        public string CurrencyName { get; set; }

        public string Wallet { get; set; }

        public string WalletName { get; set; }

        public bool IsActive { get; set; }

        public bool IsBlock { get; set; }

        public bool IsAutoSettle { get; set; }

        public override string ToString()
        {
            return $"{nameof(Amount)} : {Amount}, {nameof(CurrencyName)} : {CurrencyName}, {nameof(IsActive)} : {IsActive}";
        }
    }
}