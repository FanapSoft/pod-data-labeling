using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Fanap.DataLabeling.Clients.Pod;
using Fanap.DataLabeling.Pod;
using Microsoft.EntityFrameworkCore;

namespace Fanap.DataLabeling.Authentication
{
    // ToDo - Restructure
    public class AccessTokenManagerBase : IAccessTokenManagerBase
    {
        private readonly IRepository<ExternalToken> externalTokenRepo;
        private readonly IAbpSession session;
        protected string _token;

        public AccessTokenManagerBase(IRepository<ExternalToken> externalTokenRepo, IAbpSession session)
        {
            this.externalTokenRepo = externalTokenRepo;
            this.session = session;
        }


        public async Task<string> GetCurrentAccessToken()
        {
            if (_token == null)
            {
                await SetAccessTokenAsync();
            }

            return _token;
        }

        private async Task SetAccessTokenAsync()
        {
            if (session.UserId == null)
            {
                throw new UserFriendlyException("User is not authenticated");
            }

            if (_token != null)
            {
                return;
            }

            var externalToken = await externalTokenRepo.GetAll()
                .OrderByDescending(t => t.CreationTime)
                .FirstOrDefaultAsync(c => c.UserId == session.UserId.Value);


            if (externalToken == null)
            {
                throw new UserFriendlyException("Invalid external token");
            }

            _token = externalToken.AccessToken;
        }
    }
}