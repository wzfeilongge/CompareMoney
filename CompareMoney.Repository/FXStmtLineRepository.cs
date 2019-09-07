using CompareMoney.Core.Domain.Models;


using CompareMoney.IRepository.BaseRepository;
using CompareMoney.Repository.Base;
using Microsoft.Extensions.Logging;

namespace CompareMoney.Repository
{
   public class FXStmtLineRepository : BaseRepositoryPay<FXStmtLine>, IFXStmtLineRepository
    {


        public FXStmtLineRepository(ILogger<BaseRepositoryPay<FXStmtLine>> logger) : base(logger)
        {

        }







    }

}
