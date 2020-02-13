using BlogProjectAPI.DAL.Abstract;
using BlogProjectAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Caching.Distributed;
using ServiceStack;
using RedisException = ServiceStack.Redis.RedisException;

namespace BlogProjectAPI.DAL.Concrete.EFCore
{
    public class RedisRepository<T> : IRedisRepository<T> where T : class
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;
        public RedisRepository(DatabaseContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        string _prefix;
        List<T> _dataList = new List<T>();
        public List<T> GetAllCachedData(string cacheKey, TimeSpan expiresIn, string[] includes, bool? asNoTracking = false)
        {
            //Azure Redis dataları lokal redis e göre çok daha yavaş getiriyor.
            //asNoTracking sadece select yapılacak daha sonra modify edilmeyecek datalar için true olmalı.
            _prefix = _configuration["CachePrefix"];

            var host = _configuration.GetValue<string>("RedisConfig:Host");
            var port = _configuration.GetValue<int>("RedisConfig:Port");
            var password = _configuration.GetValue<string>("RedisConfig:Password");
            var db = _configuration.GetValue<int>("RedisConfig:Db");

            using IRedisClient client = new RedisClient(host, port, password, db);
            if (client.Ping())
            {
                var allKeys = client.SearchKeys(_prefix + cacheKey + "*");

                if (allKeys.Count > 0)
                {
                    foreach (var key in allKeys)
                    {
                        _dataList.Add(client.Get<T>(key));
                    }

                    return _dataList;
                }

                var data = asNoTracking == false ?
              includes.Length > 1 ? _context.Set<T>().Include(includes[0]).Include(includes[1])
              : _context.Set<T>().Include(includes[0])
              : includes.Length > 1 ?
                  _context.Set<T>().Include(includes[0]).Include(includes[1]).AsNoTracking()
                  : _context.Set<T>().Include(includes[0]).AsNoTracking();

                foreach (var item in data.ToList())
                {
                    var cacheData = client.As<T>();
                    cacheData.SetValue(_prefix + cacheKey + item.GetId(), item, expiresIn);
                }
                //return GetAllCachedData(cacheKey, expiresIn, includes, asNoTracking);
                return data.ToList();
            }
            throw new Exception("RedisReposity Exception");
        }
    }
}
