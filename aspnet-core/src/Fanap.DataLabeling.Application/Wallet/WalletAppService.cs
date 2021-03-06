using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Fanap.DataLabeling.Accounting;
using Fanap.DataLabeling.Authorization.Users;
using Fanap.DataLabeling.Clients.Pod;
using Fanap.DataLabeling.Clients.Pod.Dtos;
using Fanap.DataLabeling.Credit;
using Fanap.DataLabeling.DataSets;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.Wallet
{
    [AbpAuthorize]
    public class WalletAppService : DataLabelingAppServiceBase
    {
        private readonly IRepository<User, long> userRepo;
        private readonly TransactionsAppService transAppService;
        private readonly IPodClient podClient;
        private readonly IRefreshableAction action;

        public WalletAppService(IRepository<User, long> userRepo, TransactionsAppService transAppService, IPodClient podClient, IRefreshableAction action)
        {
            this.userRepo = userRepo;
            this.transAppService = transAppService;
            this.podClient = podClient;
            this.action = action;
        }

        [HttpPost]
        public async Task<TransferToUserResult> TransferCreditToWalletWithSign(TransferToUserInput input)
        {
            var userId = AbpSession.UserId.Value;
            var user = await userRepo.GetAsync(userId);
            var balance = await transAppService.GetBalance(new BalanceInput
            {
                OwnerId = userId
            });
            if (string.IsNullOrEmpty(user.PhoneNumber))
            {
                if (string.IsNullOrEmpty(input.PhoneNumber))
                    throw new UserFriendlyException("حساب کاربری شما فاقد شماره تلفن است، و وارد کردن آن الزامی است.");
                user.PhoneNumber = input.PhoneNumber;
                await userRepo.UpdateAsync(user);
            }
            var podContactId = user.PodContactId;
            if (podContactId == 0)
                throw new UserFriendlyException("This user has not been set as contact.");
            await action.TryAsync(async token => await podClient.TransferFundToContactWithSign(token, podContactId.ToString(), Convert.ToDecimal(balance.Total)));
            return new TransferToUserResult()
            {
                PhoneNumber = user.PhoneNumber
            };
        }

        [HttpPost]
        public async Task<TransferToUserResult> TransferCreditToWallet(TransferToUserInput input)
        {
            var userId = AbpSession.UserId.Value;
            var user = await userRepo.GetAsync(userId);
            var balance = await transAppService.GetBalance(new BalanceInput
            {
                OwnerId = userId
            });

            if (string.IsNullOrEmpty(user.PhoneNumber))
            {
                if (string.IsNullOrEmpty(input.PhoneNumber))
                    throw new UserFriendlyException("حساب کاربری شما فاقد شماره تلفن است، و وارد کردن آن الزامی است.");
                user.PhoneNumber = input.PhoneNumber;
                await userRepo.UpdateAsync(user);
            }

            var podContactId = user.PodContactId;
            if (podContactId == 0)
                throw new UserFriendlyException("This user has not been set as contact.");

            await podClient.TransferFundToContact(userId,podContactId.ToString(), balance);
            return new TransferToUserResult()
            {
                PhoneNumber = user.PhoneNumber
            };
        }

        [HttpPost]
        public async Task<ConfirmTransferToUserResult> ConfirmTransferCreditToWallet(ConfirmTransferToUserInput input)
        {
            var userId = AbpSession.UserId.Value;
            var user = await userRepo.GetAsync(userId);
            var podContactId = user.PodContactId;
            if (podContactId == 0)
                throw new UserFriendlyException("This user has not been set as contact.");
            var result = await podClient.ConfirmTransferFundToContact(input.PhoneNumber, input.Code);

            return new ConfirmTransferToUserResult()
            {

            };
        }
    }
}
