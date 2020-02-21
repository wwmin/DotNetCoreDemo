using System;
using System.Collections.Generic;
using System.Text;

namespace WeChatService.Model
{
    /// <summary>
    /// 返回结果
    /// </summary>
    public class Result
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool success = true;
        /// <summary>
        /// 说明
        /// </summary>
        public string message = String.Empty;
        /// <summary>
        /// 数据
        /// </summary>
        public Object data { get; set; }
    }
}
