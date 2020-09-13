using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Fanap.DataLabeling.Clients.Pod;
using Fanap.DataLabeling.Pod;
using Microsoft.EntityFrameworkCore;

namespace Fanap.DataLabeling.Authentication
{
    // ToDo - Restructure
    public class AccessTokenManagerBase : IAccessTokenManager
    {
        private readonly IRepository<ExternalToken> externalTokenRepo;
        private readonly IAbpSession session;
        private readonly IPodClient podClient;
        protected string _token;

        public AccessTokenManagerBase(IRepository<ExternalToken> externalTokenRepo,
            IAbpSession session, IPodClient podClient)
        {
            this.externalTokenRepo = externalTokenRepo;
            this.session = session;
            this.podClient = podClient;
        }
        public async Task RefreshTokenAsync()
        {
            await RefreshTokenAsync(session.UserId.Value);
        }

        [UnitOfWork]
        public async Task RefreshTokenAsync(long userId)
        {
            var externalToken = await externalTokenRepo.GetAll()
                                .OrderByDescending(et => et.Id)
                                .FirstOrDefaultAsync(et => et.UserId == userId);

            if (externalToken == null)
            {
                throw new Exception("Token not found");
            }

            var podToken = await podClient.RefreshTokenAsync(externalToken.RefreshToken);
            externalToken.UsageTime = DateTime.UtcNow;
            externalTokenRepo.Insert(
                new ExternalToken
                {
                    AccessToken = podToken.AccessToken,
                    UserId = userId,
                    CreationTime = DateTime.UtcNow,
                    Provider = "Pod",
                    RefreshToken = podToken.RefreshToken,
                });

            _token = podToken.AccessToken;
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