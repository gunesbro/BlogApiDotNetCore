using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogProjectAPI.Data.Models;
using BlogProjectAPI.Data.ViewModels;

namespace BlogProjectAPI.DAL.Abstract
{
    public interface IPostsRepository
    {
        /// <summary>
        /// örn: take 10, after:5 -> 5 kayıt atladıktan sonraki 10 kaydı getir
        /// </summary>
        /// <param name="orderBy">asc-desc</param>
        /// /// <param name="sortBy">Tablodaki sıralanması istenen alan</param>
        /// <param name="take">çekmek istenen veri miktarı. take 0 verilirse tüm data listelenir</param>
        /// <param name="skip">atlanmak istenen veri iktarı</param>
        /// <returns></returns>
        Task<List<dynamic>> GetAll(GetPostsModel model);

        Posts GetById(int id);
    }
}
