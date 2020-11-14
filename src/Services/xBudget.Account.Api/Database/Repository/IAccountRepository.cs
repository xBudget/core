using System.Threading.Tasks;
using xBudget.Account.Api.Models.Dto;
using xBudget.Lib.Database;

namespace xBudget.Account.Api.Database.Repository
{
    public interface IAccountRepository : IRepository<AccountDto>
    {
        /// <summary>
        /// Gets an Account from database using the field Name as filter.
        /// </summary>
        /// <param name="name">value for the name field.</param>
        /// <returns>an Account or null if there isn't a account with the name in parameter.</returns>
        Task<AccountDto> GetAccountByName(string name);
    }
}
