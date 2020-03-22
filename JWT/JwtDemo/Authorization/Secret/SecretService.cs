using JwtDemo.Authorization.Secret.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtDemo.Authorization.Secret
{
    public class SecretService : ISecretService
    {
        public SecretService()
        {

        }

        #region API Implements

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public UserDto GetCurrentUserAsync(string account, string password)
        {
            //从数据库获取数据然后比对用户名密码
            var user = new UserDto()
            {
                Id = 0,
                UserName = account,
                Email = "test@mail.com",
                Phone = "11111111111",
                Role = "admin"
            };

            //Todo：AutoMapper 做实体转换

            return user;
        }

        #endregion
    }
}
