using System.Threading.Tasks;
using Fanap.DataLabeling.Models.TokenAuth;
using Fanap.DataLabeling.Web.Controllers;
using Shouldly;
using Xunit;

namespace Fanap.DataLabeling.Web.Tests.Controllers
{
    public class HomeController_Tests: DataLabelingWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}