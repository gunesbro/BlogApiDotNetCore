using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using BlogProjectAPI.DAL.Abstract;
using BlogProjectAPI.Data.Models;
using BlogProjectAPI.Data.ViewModels;
using Microsoft.Extensions.Configuration;

namespace BlogProjectAPI.DAL.Concrete.EFCore
{
    public class PostsRepository: IPostsRepository
    {
        private readonly DatabaseContext _context;
        private readonly IRedisRepository<Posts> _redisRepository;
        private readonly IConfiguration _configuration;
        public PostsRepository(DatabaseContext context, IRedisRepository<Posts> redisRepository, IConfiguration configuration)
        {
            _context = context;
            _redisRepository = redisRepository;
            _configuration = configuration;
        }

        public List<Posts> dataList;
        public async Task<List<dynamic>> GetAll(GetPostsModel model)
        {
            string cacheKey = "Post";
            string[] includes = { "Authors" };
            var expireMin = _configuration.GetValue<int>("RedisConfig:CacheExpireMin");
            TimeSpan expiresIn = TimeSpan.FromMinutes(expireMin);

            dataList = _redisRepository.GetAllCachedData(cacheKey, expiresIn, includes, false);
            var filter = dataList.AsQueryable().OrderBy(model.SortBy + " " + model.OrderBy).Skip(model.Skip);
            var filtered = model.Take == 0 ? await filter.ToDynamicListAsync() : await filter.Take(model.Take).ToDynamicListAsync();
            return filtered;
        }

        public Task<Posts> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
