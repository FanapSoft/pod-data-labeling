using Abp.Dependency;
using Abp.Domain.Repositories;
using Fanap.DataLabeling.Authentication;
using Fanap.DataLabeling.Pod;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling
{
    public interface IRefreshableAction: ITransientDependency
    {
        Task<TOut> TryAsync<TOut>(Func<string, Task<TOut>> method);
    }

    public class RefreshableAction : IRefreshableAction
    {
        private readonly IAccessTokenManager _tokenManager;
        private readonly  IRepository<ExternalToken> externalTokens;

        public RefreshableAction(IAccessTokenManager tokenManager, IRepository<ExternalToken> externalTokens)
        {
            _tokenManager = tokenManager;
            this.externalTokens = externalTokens;
        }
        public async Task<TOut> TryAsync<TOut>(Func<string, Task<TOut>> method)
        {
            try
            {
                return await CallMethodAsync(method);
            }
            catch
            {
                await _tokenManager.RefreshTokenAsync();
                return await CallMethodAsync(method);
            }
        }

     
        private async Task<TOut> CallMethodAsync<TOut>(Func<string, Task<TOut>> method)
        {
            var token = await _tokenManager.GetCurrentAccessToken();
            return await method(token);
        }

    }


}
