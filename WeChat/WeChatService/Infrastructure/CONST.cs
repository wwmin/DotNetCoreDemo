using System;
using System.Collections.Generic;
using System.Text;

namespace WeChatService.Infrastructure
{
    /// <summary>
    /// 常量
    /// </summary>
    public static class Const
    {
        /// <summary>
        /// 分页header标识
        /// </summary>
        public const string X_PAGINATION = "X-Pagination";

        public const string ALLOW_SPECIFIC_ORIGIN = "AllowSpecificOrigin";

        //public const string ORIGIN_URL = "https://www.wsdscloud.com";
        public const string ORIGIN_URL = "http://localhost:8000";
        public static class WECHAT
        {
            public const string URL_ROOT = "https://api.weixin.qq.com/";

            public const string URL_PUBLIC_TOKEN = "cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            public const string URL_PUBLIC_CODE_TOKEN = "sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";
            public const string URL_PUBLIC_REFRESH_CODE_TOKEN = "sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}";

            public const string TOKEN = "ceshi";
            public const string ACCESS_TOKEN = "ACCESS_TOKEN";//基本token
            public const string ACCESS_PUBLIC_TOKEN_CODE = "ACCESS_PUBLIC_TOKEN_CODE";//公众号token 需要加code做唯一标识

            // 小程序
            public const string URL_CODE_TOKEN = "sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code";

            //模板消息推送 (从2020年1月已废弃,清使用订阅消息)
            public const string URL_TEMPLATE_MESSAGE_SEND = "cgi-bin/message/wxopen/template/send?access_token={0}";
            //订阅消息
            public const string URL_SUBSCRIBE_MESSAGE_SEND = "message/subscribe/send?access_token={0}";
        }
    }
}
