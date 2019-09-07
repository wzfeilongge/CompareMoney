using CompareMoney.Core.Domain.Models;
using CompareMoney.IRepository;
using CompareMoney.Repository.Base;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompareMoney.Repository
{
   public  class VIEW_JYMXTableRepository : BaseRepository<VIEW_JYMX>, IVIEW_JYMXTableRepository
    {
        public VIEW_JYMXTableRepository(ILogger<BaseRepository<VIEW_JYMX>> logger) : base(logger)
        {

        }
    }
}
