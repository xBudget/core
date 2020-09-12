using System.Threading.Tasks;
using xBudget.Identity.Api.Models;
using xBudget.Identity.Api.Models.Core;

namespace xBudget.Identity.Api.Services
{
    public interface IAuthService
    {
        Task<ServiceResult<string>> Login(UserLoginViewModel viewModel);
        Task<ServiceResult<string>> Register(UserRegisterViewModel viewModel);
    }
}
