using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JwtDemo.Infrastructures
{
    public static class ClaimUtil
    {
        /// <summary>
        /// 获取当前用户的CompanyId
        /// </summary>
        /// <returns></returns>
        public static int CompanyId(HttpContext context)
        {
            var contextUser = context.User;
            var CompanyId = contextUser.Claims.Where(p => p.Type == ClaimTypes.GroupSid).FirstOrDefault();
            if (CompanyId == null) throw new Exception("获取token信息内容失败");
            try
            {
                var result = int.Parse(CompanyId.Value);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取当前用户的RoleId
        /// </summary>
        /// <returns></returns>
        public static int RoleId(HttpContext context)
        {
            var contextUser = context.User;
            var RoleId = contextUser.Claims.Where(p => p.Type == ClaimTypes.Sid).FirstOrDefault();
            if (RoleId == null) throw new Exception("获取token信息内容失败");
            try
            {
                var result = int.Parse(RoleId.Value);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取当前用户的RoleName
        /// </summary>
        /// <returns></returns>
        public static string RoleName(HttpContext context)
        {
            var contextUser = context.User;
            var RoleName = contextUser.Claims.Where(p => p.Type == ClaimTypes.Role).FirstOrDefault();
            if (RoleName == null) throw new Exception("获取token信息内容失败");
            return RoleName.Value;
        }

        /// <summary>
        /// 获取当前用户的UserId
        /// </summary>
        /// <returns></returns>
        public static int UserId(HttpContext context)
        {
            var contextUser = context.User;
            var UserId = contextUser.Claims.Where(p => p.Type == ClaimTypes.NameIdentifier).FirstOrDefault();
            if (UserId == null) throw new Exception("获取token信息内容失败");
            try
            {
                var result = int.Parse(UserId.Value);
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取当前用户的UserName
        /// </summary>
        /// <returns></returns>
        public static string UserName(HttpContext context)
        {
            var contextUser = context.User;
            var UserId = contextUser.Claims.Where(p => p.Type == ClaimTypes.Name).FirstOrDefault();
            if (UserId == null) throw new Exception("获取token信息内容失败");
            return UserId.Value;
        }

        /// <summary>
        /// 获取用户年龄
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int UserAge(HttpContext context)
        {
            var contextUser = context.User;
            var userAge = contextUser.Claims.Where(p => p.Type == ClaimTypes.DateOfBirth).FirstOrDefault();
            if (userAge == null) throw new Exception("获取token信息内容失败");
            DateTime.TryParse(userAge.Value, out var dateOfBirth);
            int age = dateOfBirth.Year >= 1970 ? DateTime.Today.Year - dateOfBirth.Year : 0;
            return age;
        }
    }
}
