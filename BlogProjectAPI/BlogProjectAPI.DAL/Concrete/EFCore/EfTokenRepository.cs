using BlogProjectAPI.DAL.Abstract;
using BlogProjectAPI.Data.Models;
using BlogProjectAPI.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BlogProjectAPI.DAL.Concrete.EFCore
{
    public class EfTokenRepository : ITokenRepository
    {
        private readonly DatabaseContext _context;
        private readonly IRedisRepository<TokenUser> _redisRepository;
        private readonly IConfiguration _configuration;
        public EfTokenRepository(DatabaseContext context, IRedisRepository<TokenUser> redisRepository, IConfiguration configuration)
        {
            this._context = context;
            this._redisRepository = redisRepository;
            _configuration = configuration;
        }
        /*
            * List in ilk dolumundan sonra, Cache deki veriler expire olana kadar tekrar cache den data çekmemek için
            * Static bir List(model adında) oluşturuldu. Cache expire olduktan 1 dakika sonra List tekrar cache den dolacak
            * şekilde bir kurgu oluşturuldu.
            */
        private static List<TokenUser> _model;
        private static DateTime _time = DateTime.Now;
        public async Task<TokenUser> CheckUserAndGetInfoAysnc(TokenLoginModel loginModel)
        {
            var expireMin = _configuration.GetValue<int>("RedisConfig:CacheExpireMin");
            var cacheKey = "TokenUser";
            string[] includes = { "Roles" };
            TimeSpan expiresIn = TimeSpan.FromMinutes(expireMin);

            if (_time.AddMinutes(expireMin + 1) < DateTime.Now)
            { _model = null; _time = DateTime.Now; }

            if (_model == null)
            {
                _model = _redisRepository.GetAllCachedData(cacheKey, expiresIn, includes, true);
                if (_model != null)
                {
                    var find = _model.FirstOrDefault(user => user.Username == loginModel.Username && user.Password == loginModel.Password && user.IsSuspended == false);
                    return find;
                }
                //redis patlarsa
                _model = await _context.TokenUsers.
                                Include(i => i.Roles).AsNoTracking(). //// Microsoft.EntityFrameworkCore.Proxies paketini indirip UseLazyLoadingProxies() methodu da kullanılabilir
                                ToListAsync();
            }
            var findFaster = (_model ?? throw new InvalidOperationException()).FirstOrDefault(user => user.Username == loginModel.Username && user.Password == loginModel.Password && user.IsSuspended == false);
            return findFaster;

        }
    }
}
