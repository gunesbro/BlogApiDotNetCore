using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BlogProjectAPI.DAL.Abstract;
using BlogProjectAPI.Data.ViewModels;
using BlogProjectAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;


namespace BlogProjectAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {

        IConfiguration _configuration;
        ITokenRepository _tokenRepository;
        public TokenController(ITokenRepository tokenRepository, IConfiguration configuration)
        {
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Herhangi bir Methoda istek yapmak için ilk olarak bu method ile Token almalısınız.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("GetToken")]
        public async Task<IActionResult> GetToken([FromBody]TokenLoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _tokenRepository.CheckUserAndGetInfoAysnc(loginModel);
                if (user != null)
                {
                    string token = TokenHelper.GenerateToken(_configuration, user);
                    //TODO:Girişi logla. Db ye kaydedilebilir takibi kolay olacaktır.
                    //TokenHelper.LogThisAccess(loginModel,true, "Approved");
                    return Ok(new { User = user.Username, Access = "Approved", Token = token });
                }
                //TokenHelper.LogThisAccess(loginModel,false, "Not Approved");
                return BadRequest(new { User = loginModel.Username, Access = "Not Approved" });
            }
            return BadRequest();

        }
    }
}