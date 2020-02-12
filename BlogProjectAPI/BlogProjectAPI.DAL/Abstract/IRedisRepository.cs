using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BlogProjectAPI.Data.Models;

namespace BlogProjectAPI.DAL.Abstract
{
    public interface IRedisRepository<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheKey">Cache anahar kelimesi</param>
        /// <param name="expiresIn">Cache silinme süresi</param>
        /// <param name="includes">İlişkili tablolar</param>
        /// <param name="asNoTracking"></param>
        /// <returns></returns>
        List<T> GetAllCachedData(string cacheKey,TimeSpan expiresIn, string[] includes, bool? asNoTracking);
    }


}
