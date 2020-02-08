using BlogProjectAPI.DAL.Abstract;
using BlogProjectAPI.Data.Models;
using BlogProjectAPI.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogProjectAPI.DAL.Concrete.EFCore
{
    public class EfTokenRepository : ITokenRepository
    {
        private readonly DatabaseContext _context;
        private readonly IRedisRepository<TokenUser> _redisRepository;
        public EfTokenRepository(DatabaseContext context, IRedisRepository<TokenUser> redisRepository)
        {
            this._context = context;
            this._redisRepository = redisRepository;
        }

        private static List<TokenUser> model;
        private static DateTime time = DateTime.Now;
        public async Task<TokenUser> CheckUserAndGetInfoAysnc(TokenLoginModel loginModel)
        {
            int expireMin = 29;
            var cacheKey = "TokenUser";
            string[] includes = { "Roles" };
            TimeSpan expiresIn = TimeSpan.FromMinutes(expireMin);
            /*
            * List in ilk dolumundan sonra, Cache deki veriler expire olana kadar tekrar cache den data çekmemek için
            * Static bir List(model adında) oluşturuldu. Cache expire olduktan 1 dakika sonra List tekrar cache den dolacak
            * şekilde bir kurgu oluşturuldu.
            */
            try
            {

                if (time.AddMinutes(expireMin + 1) < DateTime.Now)
                {
                    model = null; time = DateTime.Now;
                }

                if (model == null)
                {
                    model = _redisRepository.GetAllCachedData(cacheKey, expiresIn, includes, true);
                    if (model != null)
                    {
                        var find = model.FirstOrDefault(user => user.Username == loginModel.Username && user.Password == loginModel.Password);
                        return find;
                    }
                    //redis patlarsa
                    model = await _context.TokenUsers.
                                    Include(i => i.Roles).AsNoTracking(). //// Microsoft.EntityFrameworkCore.Proxies paketini indirip UseLazyLoadingProxies() methodu da kullanılabilir
                                    ToListAsync();
                }
                var findFaster = (model ?? throw new InvalidOperationException()).FirstOrDefault(user => user.Username == loginModel.Username && user.Password == loginModel.Password);
                return findFaster;

            }
            catch (Exception ex)
            {
                //Aman ağzımızın tadı kaçmasın
                Console.Write(ex.Message.ToList());
            }

            return null;
        }
    }
}
