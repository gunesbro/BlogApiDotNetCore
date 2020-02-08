using System;
using System.Collections.Generic;
using System.Text;
using BlogProjectAPI.Data.Models;

namespace BlogProjectAPI.DAL.Abstract
{
    public interface IRedisRepository<T> where T : class
    {
        List<T> GetAllCachedData(string cacheKey,TimeSpan expiresIn, string[] includes, bool? asNoTracking);
    }


}
