using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using xBudget.Account.Api.Database.Repository;
using xBudget.Account.Api.Models.Api;
using xBudget.Account.Api.Models.Dto;

namespace xBudget.Account.Api.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateAccountApiModel createAccountViewModel)
        {
            try
            {
                await _accountRepository.Create(new AccountDto
                {
                    Active = createAccountViewModel.Active,
                    Description = createAccountViewModel.Description,
                    Name = createAccountViewModel.Name
                });

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }            
        }
    }
}
