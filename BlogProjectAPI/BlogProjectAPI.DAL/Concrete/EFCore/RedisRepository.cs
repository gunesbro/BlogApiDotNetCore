using BlogProjectAPI.DAL.Abstract;
using BlogProjectAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Caching.Distributed;
using RedisException = ServiceStack.Redis.RedisException;

namespace BlogProjectAPI.DAL.Concrete.EFCore
{
    public class RedisRepository<T> : IRedisRepository<T> where T : class 
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        private IDistributedCache _cache;
        public RedisRepository(DatabaseContext context, IConfiguration configuration,IDistributedCache cache)
        {
            _context = context;
            _configuration = configuration;
            _cache = cache;
        }

        string _prefix;
        List<T> _dataList = new List<T>();
        public List<T> GetAllCachedData(string cacheKey, TimeSpan expiresIn, string[] includes, bool? asNoTracking = false)
        {
            //asNoTracking sadece select yapılacak daha sonra modify edilmeyecek datalar için true olmalı.
            _prefix = _configuration["CachePrefix"];
            try
            {
                using IRedisClient client = new RedisClient(_configuration["Host"],int.Parse(_configuration["Port"]), _configuration["Password"],int.Parse(_configuration["Db"]));
                if (client.Ping())
                {
                    List<string> allKeys = client.SearchKeys(_prefix + cacheKey + "*"); 
                    
                    if (allKeys.Count > 0)
                    {
                        foreach (var key in allKeys)
                        {
                            _dataList.Add(client.Get<T>(key));
                        }

                        return _dataList;
                    }
                    else
                    {
                        var data = asNoTracking == false ?
                                includes.Length > 1 ? _context.Set<T>().Include(includes[0]).Include(includes[1])
                                : _context.Set<T>().Include(includes[0])
                            : includes.Length > 1 ?
                                _context.Set<T>().Include(includes[0]).Include(includes[1]).AsNoTracking()
                                : _context.Set<T>().Include(includes[0]).AsNoTracking();

                        foreach (var item in data.ToList())
                        {
                            var cacheData = client.As<T>();
                            cacheData.SetValue(_prefix + cacheKey + Guid.NewGuid().ToString("N"), item, expiresIn);
                        }
                        //return GetAllCachedData(cacheKey, expiresIn, includes, asNoTracking);
                        return data.ToList();
                    }
                }

                return null;

            }
            catch (RedisException ex)
            {
                //Redis patlarsa buralar yanar
                Console.Write(ex.Message);
                
                return null;
            }

        }
    }
}
