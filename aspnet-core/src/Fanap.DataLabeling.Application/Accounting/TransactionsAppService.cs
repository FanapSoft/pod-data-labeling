using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Fanap.DataLabeling.Accounting;
using Fanap.DataLabeling.Authorization;
using Fanap.DataLabeling.Datasets;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanap.DataLabeling.DataSets
{
    public class BalanceInput
    {

        [Required]
        public long OwnerId { get; set; }
    }
    public class BalanceOutput
    {
        public double DebitAmount { get; set; }
        public double CreditAmount { get; set; }
        public double Total { get; set; }
    }
    public class TransactionsGetAllInput : PagedAndSortedResultRequestDto
    {
        public long? OwnerId { get; set; }
        public Guid? DataSetId { get; set; }
    }
    [AbpAuthorize]
    public class TransactionsAppService : AsyncCrudAppService<Transaction, TransactionDto, Guid, TransactionsGetAllInput>
    {
        
        public TransactionsAppService(IRepository<Transaction, Guid> repository) : base(repository)
        {
            CreatePermissionName = PermissionNames.Pages_Roles;
            UpdatePermissionName = PermissionNames.Pages_Roles;
            DeletePermissionName = PermissionNames.Pages_Roles;
        }
        protected override IQueryable<Transaction> CreateFilteredQuery(TransactionsGetAllInput input)
        {
            if (input.OwnerId == null)
                CheckPermission(PermissionNames.Pages_Roles);
            if (input.OwnerId != null && input.OwnerId.Value != AbpSession.UserId.Value)
                CheckPermission(PermissionNames.Pages_Roles);

            return base.CreateFilteredQuery(input)
                .WhereIf(input.DataSetId != null, ff => ff.ReferenceDataSetId == input.DataSetId.Value)
                .WhereIf(input.OwnerId != null, ff => ff.OwnerId == input.OwnerId.Value);
        }
        public async Task<BalanceOutput> GetBalance(BalanceInput input)
        {
            var totalDebit = await Repository.GetAll().Where(ff => ff.OwnerId == input.OwnerId).SumAsync(ff => ff.DebitAmount);
            var totalCredit = await Repository.GetAll().Where(ff => ff.OwnerId == input.OwnerId).SumAsync(ff => ff.CreditAmount);
            return new BalanceOutput
            {
                CreditAmount = totalCredit,
                DebitAmount = totalDebit,
                Total = totalCredit - totalDebit
            };
        }
    }
}
