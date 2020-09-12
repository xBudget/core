using System.Collections.Generic;

namespace xBudget.Identity.Api.Models.Core
{
    public class ServiceResult<TResult>
    {
        public bool Success { get { return Errors == null || Errors.Count == 0; } }
        public TResult Result { get; set; }
        public IList<string> Errors { get; set; }
    }
}
