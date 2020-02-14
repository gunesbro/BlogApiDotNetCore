using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlogProjectAPI.Data.Models;

namespace BlogProjectAPI.DAL.Abstract
{
    public interface IAccessLoggerRepository
    {
        Task LogThisAccess(AccessLogs model);
    }
}
