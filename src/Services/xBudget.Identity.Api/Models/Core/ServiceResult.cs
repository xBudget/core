using System.Collections.Generic;

namespace xBudget.Identity.Api.Models.Core
{
    public class ServiceResult<TResult>
    {
        public bool Success { get { return Errors.Count == 0; } }
        public TResult Result { get; set; }
        public Dictionary<string, IList<string>> Errors { get; set; }

        public ServiceResult()
        {
            Errors = new Dictionary<string, IList<string>>();
        }
    }
}
