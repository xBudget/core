using System.ComponentModel.DataAnnotations;

namespace xBudget.Account.Api.Models.Api
{
    public class CreateAccountApiModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public bool Active { get; set; }
    }
}
