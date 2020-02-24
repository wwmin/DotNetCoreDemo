using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Models;
using IdentityServer.Services;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace IdentityServer.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;

        private IAdminService _adminService;//自己写的操作数据库Admin表的service
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        public AccountController(IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IAdminService adminService, 
            ILogger<AccountController> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _adminService = adminService;
            _logger = logger;
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        /// <summary>
        /// 登录post回发处理
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            Admin user = await _adminService.GetByStr(userName, password);
            if (user != null)
            {
                AuthenticationProperties props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(1))
                };
                
                await HttpContext.SignInAsync(user.Id.ToString(), user.UserName, props);
                if (returnUrl != null)
                {
                    return Redirect(returnUrl);
                }

                return View();
            }
            else
            {
                return View();
            }
        }
    }
}