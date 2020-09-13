using Abp.Authorization;
using Fanap.DataLabeling.Clients.Pod;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Wallet
{
    [AbpAuthorize]
    public class WalletAppService: DataLabelingAppServiceBase
    {
        private readonly IPodClient podClient;
        private readonly IRefreshableAction action;

        public WalletAppService(IPodClient podClient, IRefreshableAction action)
        {
            this.podClient = podClient;
            this.action = action;
        }

        public async Task<PodWalletCreditDto> GetCreditAsync()
        {
            return await action.TryAsync(token => podClient.GetWalletCredit(token));
        }

    }
}
