using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace xBudget.Lib.Database
{
    public interface IRepository<TViewModel>
    {
        Task Create(TViewModel viewModel);

        Task<TViewModel> Get(Guid Id);

        Task<List<TViewModel>> List(int skip = 0, int take = 20);
    }
}
