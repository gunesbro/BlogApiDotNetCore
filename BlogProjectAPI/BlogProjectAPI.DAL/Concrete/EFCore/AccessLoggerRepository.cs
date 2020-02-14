using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BlogProjectAPI.DAL.Abstract;
using BlogProjectAPI.Data.Models;

namespace BlogProjectAPI.DAL.Concrete.EFCore
{
    public class AccessLoggerRepository:IAccessLoggerRepository
    {
        private readonly DatabaseContext _context;
        public AccessLoggerRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task LogThisAccess(AccessLogs model)
        {
            await _context.AccessLogs.AddAsync(model);
            await _context.SaveChangesAsync();
        }
    }
}
