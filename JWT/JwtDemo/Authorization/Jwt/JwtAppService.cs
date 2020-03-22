using JwtDemo.Authorization.Secret.Dto;
using JwtDemo.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JwtDemo.Authorization.Jwt
{
    public class JwtAppService : IJwtAppService
    {
        #region Initialize
        /// <summary>
        /// 已授权的Token信息集合
        /// </summary>
        private static ISet<JwtAuthorizationDto> _tokens = new HashSet<JwtAuthorizationDto>();

        /// <summary>
        /// 分布式缓存
        /// </summary>
        private readonly IDistributedCache _cache;
        /// <summary>
        /// 配置信息
        /// </summary>
        private readonly IConfiguration _configuration;

        private readonly JwtOptions _jwtOptions;
        /// <summary>
        /// 获取HTTP请求上下文
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAppService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IOptions<JwtOptions> jwtOptions)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _jwtOptions = jwtOptions.Value;
        }
        #endregion

        #region API Implements
        /// <summary>
        /// 新增Token
        /// </summary>
        /// <param name="dto">用户信息数据传输对象</param>
        /// <returns></returns>
        public JwtAuthorizationDto Create(UserDto dto)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecurityKey));

            DateTime authTime = DateTime.UtcNow;
            DateTime expiresAt = authTime.AddMinutes(_jwtOptions.ExpireMinutes);

            //将用户信息添加到Claims中
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            IEnumerable<Claim> claims = new Claim[]
            {
                new Claim(ClaimTypes.Name,dto.UserName),
                new Claim(ClaimTypes.Role,dto.Role),
                new Claim(ClaimTypes.Email,dto.Email),
                new Claim(ClaimTypes.Expiration,expiresAt.ToString())
            };

            identity.AddClaims(claims);

            //签发一个加密后的用户信息凭证,用来标识用户的身份
            _httpContextAccessor.HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),//创建声明信息
                Issuer = _jwtOptions.Issuer,//jwt token的签发者
                Audience = _jwtOptions.Audience,//Jwt token 的接收者
                Expires = expiresAt,//过期时间
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            };
            //创建token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //存储token信息
            var jwt = new JwtAuthorizationDto
            {
                UserId = dto.Id,
                Token = tokenHandler.WriteToken(token),
                Auths = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                Expires = new DateTimeOffset(expiresAt).ToUnixTimeSeconds(),
                Success = true
            };
            _cache.SetStringAsync(GetKey(jwt.Token), jwt.Token, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_jwtOptions.ExpireMinutes)
            });
            _tokens.Add(jwt);
            return jwt;
        }

        /// <summary>
        /// 停用Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task DeactivateAsync(string token)
        {
            //await _cache.SetStringAsync(GetKey(token), null, new DistributedCacheEntryOptions
            //{
            //    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_jwtOptions.ExpireMinutes)
            //});
            await _cache.RemoveAsync(GetKey(token));
        }

        /// <summary>
        /// 停用当前token
        /// </summary>
        /// <returns></returns>
        public async Task DeactivateCurrentAsync() => await DeactivateAsync(GetCurrentAsync());

        /// <summary>
        /// 判断 Token 是否有效
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> IsActiveAsync(string token)
        {
            var cacheToken = await _cache.GetStringAsync(GetKey(token));
            return cacheToken != null;
        }

        /// <summary>
        /// 判断当前Token是否有效
        /// </summary>
        /// <returns></returns>
        public async Task<bool> IsCurrentActiveTokenAsync() => await IsActiveAsync(GetCurrentAsync());

        /// <summary>
        /// 刷新Token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<JwtAuthorizationDto> RefreshAsync(string token, UserDto dto)
        {
            var jwtOld = GetExistenceToken(token);
            if (jwtOld == null)
            {
                return new JwtAuthorizationDto()
                {
                    Token = "未获取到当前Token信息",
                    Success = false
                };
            }

            var jwt = Create(dto);
            //停用修改前的Token信息
            await DeactivateCurrentAsync();
            return jwt;
        }
        #endregion

        #region Method
        /// <summary>
        /// 设置缓存中过期Token值的key
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private static string GetKey(string token) => $"token:{token}";
        /// <summary>
        /// 获取HTTP请求的Token值
        /// </summary>
        /// <returns></returns>
        private string GetCurrentAsync()
        {
            //http header 
            var authorizationHeader = _httpContextAccessor.HttpContext.Request.Headers["authorization"];

            //token
            return authorizationHeader == StringValues.Empty ? string.Empty : authorizationHeader.Single().Split(" ").Last();//Bearer token
        }
        /// <summary>
        /// 判断是否存在当前Token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private JwtAuthorizationDto GetExistenceToken(string token) => _tokens.SingleOrDefault(x => x.Token == token);
        #endregion
    }
}
