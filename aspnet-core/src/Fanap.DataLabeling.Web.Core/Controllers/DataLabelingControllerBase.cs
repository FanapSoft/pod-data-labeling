using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Fanap.DataLabeling.Controllers
{
    public abstract class DataLabelingControllerBase: AbpController
    {
        protected DataLabelingControllerBase()
        {
            LocalizationSourceName = DataLabelingConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
