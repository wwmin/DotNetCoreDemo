using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWeb1
{
    public class LoginInput
    {
        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "用户名")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0}长度错误")]
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "{0}不能为空")]
        [Display(Name = "密码")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "{0}长度错误")]
        public string Password { get; set; }
    }
}
