using BlogProjectAPI.Data.Models;
using BlogProjectAPI.Data.ViewModels;
using System.Threading.Tasks;

namespace BlogProjectAPI.DAL.Abstract
{
    public interface ITokenRepository
    {
        Task<TokenUser> CheckUserAndGetInfoAysnc(TokenLoginModel loginModel);
    }
}
