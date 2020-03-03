using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SignalRWeb1.Models.Dto;

namespace SignalRWeb1.Controllers
{
    /// <summary>
    /// 账户
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _log;
        public AccountController(ILogger<AccountController> log)
        {
            _log = log;
        }

        /// <summary>
        /// 登录验证 (用户名密码登录)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        public ActionResult Login([FromBody]LoginInput input)
        {


            _log.LogInformation($"登录：{input.UserName}|{HttpContext.Connection.RemoteIpAddress.ToString()}");
            var authClaims = new[] {
                new Claim(ClaimTypes.Name,input.UserName)
                //new Claim(ClaimTypes.Role,res.role.NormalizedName),
                //new Claim(ClaimTypes.Sid,user.RoleId.ToString()),
                //new Claim(ClaimTypes.PrimaryGroupSid,user.CompanyId.ToString())
            };
            var userDto = new UserDto
            {
                Id = 0,
                Name = input.UserName
            };

            return Ok(userDto);
        }

    }
}