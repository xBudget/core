using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xBudget.Account.Api.Models.Dto;

namespace xBudget.Account.Api.Database.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly AccountDatabaseContext _accountDatabaseContext;

        public AccountRepository(AccountDatabaseContext accountDatabaseContext)
        {
            _accountDatabaseContext = accountDatabaseContext ?? throw new ArgumentNullException(nameof(accountDatabaseContext));
        }

        /// <summary>
        /// Creates a new Account in database.
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public async Task Create(AccountDto viewModel)
        {
            await _accountDatabaseContext.Accounts.AddAsync(new Models.Database.Account
            {
                Active = viewModel.Active,
                Description = viewModel.Description,
                Id = viewModel.Id,
                Name = viewModel.Name
            });

            await _accountDatabaseContext.SaveChangesAsync();
        }

        /// <summary>
        /// Gets the Account data using the id field as seach criteria.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns>an Account or null if there isn't a account with the id in parameter.</returns>
        public async Task<AccountDto> Get(Guid Id)
        {
            var account = await _accountDatabaseContext.Accounts.SingleOrDefaultAsync(x => x.Id == Id);

            if (account == null)
            {
                return null;
            }

            return new AccountDto
            {
                Id = account.Id,
                Active = account.Active,
                Description = account.Description,
                Name = account.Name
            };
        }

        /// <summary>
        /// Gets an Account from database using the field Name as filter.
        /// </summary>
        /// <param name="name">value for the name field.</param>
        /// <returns>an Account or null if there isn't a account with the name in parameter.</returns>
        public async Task<AccountDto> GetAccountByName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            var account = await _accountDatabaseContext.Accounts.SingleOrDefaultAsync(x => x.Name == name);

            // returns null if there isn't an account with the name in database.
            if (account == null)
            {
                return null;
            }

            return new AccountDto
            {
                Id = account.Id,
                Active = account.Active,
                Description = account.Description,
                Name = account.Name
            };
        }

        /// <summary>
        /// Gets a list of accounts.
        /// </summary>
        /// <param name="skip">the number of rows to skip. Default value is 0.</param>
        /// <param name="take">the number of rows to take. Default value is 20.</param>
        /// <returns></returns>
        public async Task<List<AccountDto>> List(int skip = 0, int take = 20)
        {
            var accounts = await _accountDatabaseContext.Accounts.Skip(skip).Take(take).Select(x => new AccountDto
            {
                Active = x.Active,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name
            }).ToListAsync();

            return accounts;
        }
    }
}
