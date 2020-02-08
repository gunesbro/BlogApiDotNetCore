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
        public TokenController(ITokenRepository tokenRepository,IConfiguration configuration)
        {
            _tokenRepository = tokenRepository;
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("GetToken")]
        public async Task<IActionResult> GetToken([FromBody]TokenLoginModel loginModel)
        {
            try
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

                    //Kullanıcı bulunamadı.Hatalı giriş olabilir. Loglanabilir. Arka arkaya fazla yanlış deneme olursa kullanıcıyı suspend et.
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                //Hataya düşüren requesti kaydetmek debuging için yararlı olacaktır.
                //ExceptionLogs exceptionLogs = new ExceptionLogs(); 

                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                return StatusCode(500, 
                    "An error occured. Please contact with your Admin. Status Code: 0000x0. \n" + st+ "\n" + frame + "\n" + line + "\n" + ex.Message);
            }
        }
    }
}