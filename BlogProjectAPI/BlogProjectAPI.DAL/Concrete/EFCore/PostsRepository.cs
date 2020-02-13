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


        public async Task<List<dynamic>> GetAll(GetPostsModel model)
        {
            var data = GetPostList();
            var filter = data.AsQueryable().OrderBy(model.SortBy + " " + model.OrderBy).Skip(model.Skip);
            var filtered = model.Take == 0 ? await filter.ToDynamicListAsync() : await filter.Take(model.Take).ToDynamicListAsync();
            return filtered;
        }

        public List<Posts> GetPostList()
        {
            string cacheKey = "Post";
            string[] includes = { "Authors" };
            var expireMin = _configuration.GetValue<int>("RedisConfig:CacheExpireMin");
            TimeSpan expiresIn = TimeSpan.FromMinutes(expireMin);
            var dataList = _redisRepository.GetAllCachedData(cacheKey, expiresIn, includes, false);
            return dataList;
        }
        public Posts GetById(int id)
        {
            var data = GetPostList();
            var findById = data.FirstOrDefault(x => x.PostId == id);
            return findById;
        }
    }
}
