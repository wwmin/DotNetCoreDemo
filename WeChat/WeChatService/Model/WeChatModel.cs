using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WeChatService.Model
{
    /// <summary>
    /// 微信相关类
    /// </summary>
    public class WeChatModel
    {
        /// <summary>
        /// 微信公众账号请求参数
        /// </summary>
        public class CheckSignatureCondition
        {
            /// <summary>
            /// appid
            /// </summary>
            public string appid { get; set; }
            /// <summary>
            /// 微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。
            /// </summary>
            [Required]
            public string signature { get; set; }
            /// <summary>
            /// 时间戳
            /// </summary>
            [Required]
            public string timestamp { get; set; }
            /// <summary>
            /// 随机数
            /// </summary>
            [Required]
            public string nonce { get; set; }
            /// <summary>
            /// 随机字符串
            /// </summary>
            [Required]
            public string echostr { get; set; }
        }

        public class AccessTokenResult
        {
            /// <summary>
            /// 获取到的凭证
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 凭证有效时间，单位：秒
            /// </summary>
            public int expires_in { get; set; }
            /// <summary>
            /// 错误码
            /// -1	系统繁忙，此时请开发者稍候再试	
            ///0	请求成功	
            ///40001	AppSecret 错误或者 AppSecret 不属于这个小程序，请开发者确认 AppSecret 的正确性	
            ///40002	请确保 grant_type 字段值为 client_credential	
            ///40013	不合法的 AppID，请开发者检查 AppID 的正确性，避免异常字符，注意大小写
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }
        }



        public class AccessCodeTokenCondition
        {
            [MinLength(1)]
            [MaxLength(50)]
            [Required]
            public string appid { get; set; }

            [MinLength(1)]
            [MaxLength(50)]
            [Required]
            public string code { get; set; }
        }

        public class AccessPublicCodeTokenResult
        {
            /// <summary>
            /// 获取到的凭证
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 凭证有效时间，单位：秒
            /// </summary>
            public int expires_in { get; set; }
            /// <summary>
            /// 用户刷新access_token
            /// </summary>
            public string refresh_token { get; set; }
            /// <summary>
            /// 用户唯一标识，请注意，在未关注公众号时，用户访问公众号的网页，也会产生一个用户和公众号唯一的OpenID
            /// </summary>
            public string openid { get; set; }
            /// <summary>
            /// 用户授权的作用域，使用逗号（,）分隔
            /// </summary>
            public string scope { get; set; }
        }


        /// <summary>
        /// wechat get token result
        /// </summary>
        public class AccessWeChatCodeTokenResult
        {
            /// <summary>
            /// 用户唯一标识
            /// </summary>
            public string openid { get; set; }
            /// <summary>
            /// 会话密钥
            /// </summary>
            public string session_key { get; set; }

            /// <summary>
            /// 用户在开放平台的唯一标识符，在满足 UnionID 下发条件的情况下会返回
            /// </summary>
            public string unionid { get; set; }
            /// <summary>
            /// 错误码 
            /// -1 系统繁忙，此时请开发者稍候再试
            /// 0  请求成功
            /// 40029 code 无效
            /// 45011 频率限制，每个用户每分钟100次
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }
        }

        /// <summary>
        /// 错误返回数据结构
        /// </summary>
        public class ErrorResult
        {
            public string errcode { get; set; }
            public string errmsg { get; set; }
        }

        /// <summary>
        /// 错误返回数据结构
        /// </summary>
        public class CheckCodeCondition
        {
            [Required]
            public string phoneNum { get; set; }
            [Required]
            public string checkCode { get; set; }
            [Required]
            public string openId { get; set; }
        }
        /// <summary>
        /// 微信服务端消息推送验证接口model
        /// </summary>
        public class SignatureConditionModel
        {
            /// <summary>
            /// 微信推送配置中填写的token
            /// </summary>
            public string token { get; set; }
            /// <summary>
            /// 随机字符串
            /// </summary>
            public string echostr { get; set; }
            /// <summary>
            /// 签名
            /// </summary>
            public string signature { get; set; }
            /// <summary>
            /// 时间戳
            /// </summary>
            public string timestamp { get; set; }
            /// <summary>
            /// 随机数
            /// </summary>
            public string nonce { get; set; }

        }
        /// <summary>
        /// 模板消息发送 数据model
        /// </summary>
        public class MessageSendCondition
        {
            /// <summary>
            /// 接口调用凭证 TOKEN
            /// 必填
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 接收者（用户）的 openid
            /// 必填
            /// </summary>
            public string touser { get; set; }
            /// <summary>
            /// 所需下发的模板消息的id
            /// 必填
            /// </summary>
            public string template_id { get; set; }
            /// <summary>
            /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转。
            /// 不必填
            /// </summary>
            public string page { get; set; }
            /// <summary>
            /// 表单提交场景下，为 submit 事件带上的 formId；支付场景下，为本次支付的 prepay_id
            /// 必填
            /// </summary>
            public string form_id { get; set; }
            /// <summary>
            /// 模板内容，不填则下发空模板。具体格式请参考示例。
            /// 不必填
            /// 形式入:
            /// {
            ///    "keyword1": {
            ///        "value": "339208499", 
            ///        "color": "#173177"
            ///    }, 
            ///    "keyword2": {
            ///        "value": "2019年01月01日 12:30", 
            ///        "color": "#173177"
            ///    }
            /// }
            /// </summary>
            public object data { get; set; }
            /// <summary>
            /// 模板需要放大的关键词，不填则默认无放大
            /// 不必填
            /// </summary>
            public string emphasis_keyword { get; set; }
        }
        /// <summary>
        /// 发送模板消息后的返回值
        /// </summary>
        public class MessageSendResult
        {
            /// <summary>
            /// 错误码
            /// 40037	template_id不正确	
            /// 41028	form_id不正确，或者过期	
            /// 41029	form_id已被使用	
            /// 41030	page不正确	
            /// 45009	接口调用超过限额（目前默认每个帐号日调用限额为100万）
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }
        }

        /// <summary>
        /// 订阅消息发送参数
        /// </summary>
        public class SubscribeMessageSendCondition
        {
            /// <summary>
            /// 接口调用凭证 (必填)
            /// </summary>
            public string access_token { get; set; }
            /// <summary>
            /// 接收者（用户）的 openid (必填)
            /// </summary>
            public string touser { get; set; }
            /// <summary>
            /// 所需下发的订阅模板id (必填)
            /// </summary>
            public string template_id { get; set; }
            /// <summary>
            /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index?foo=bar）。该字段不填则模板无跳转。
            /// </summary>
            public string page { get; set; }
            /// <summary>
            /// 模板内容，格式形如 { "key1": { "value": any }, "key2": { "value": any } }
            /// </summary>
            public MessageDataObject data { get; set; }
            /// <summary>
            /// 跳转小程序类型：developer为开发版；trial为体验版；formal为正式版；默认为正式版
            /// </summary>
            public string miniprogram_state { get; set; }
            /// <summary>
            /// 进入小程序查看”的语言类型，支持zh_CN(简体中文)、en_US(英文)、zh_HK(繁体中文)、zh_TW(繁体中文)，默认为zh_CN
            /// </summary>
            public string lang { get; set; }
        }

        /// <summary>
        /// MessageDataObject
        /// </summary>
        public class MessageDataObject
        {
            public MessageValue name1 { get; set; }
            public MessageValue name2 { get; set; }
            public MessageValue name3 { get; set; }
            public MessageValue name4 { get; set; }
            public MessageValue name5 { get; set; }
            public MessageValue name6 { get; set; }
            public MessageValue name7 { get; set; }
            public MessageValue name8 { get; set; }
            public MessageValue name9 { get; set; }
            public MessageValue name10 { get; set; }

            public MessageValue time1 { get; set; }
            public MessageValue time2 { get; set; }
            public MessageValue time3 { get; set; }
            public MessageValue time4 { get; set; }
            public MessageValue time5 { get; set; }
            public MessageValue time6 { get; set; }
            public MessageValue time7 { get; set; }
            public MessageValue time8 { get; set; }
            public MessageValue time9 { get; set; }
            public MessageValue time10 { get; set; }

            public MessageValue thing1 { get; set; }
            public MessageValue thing2 { get; set; }
            public MessageValue thing3 { get; set; }
            public MessageValue thing4 { get; set; }
            public MessageValue thing5 { get; set; }
            public MessageValue thing6 { get; set; }
            public MessageValue thing7 { get; set; }
            public MessageValue thing8 { get; set; }
            public MessageValue thing9 { get; set; }
            public MessageValue thing10 { get; set; }
        }

        public class MessageValue
        {
            public string value { get; set; }
        }
        /// <summary>
        /// 订阅消息返回值
        /// </summary>
        public class SubscribeMessageSendResult
        {
            /// <summary>
            /// 错误码 
            /// 40003	touser字段openid为空或者不正确	
            /// 40037	订阅模板id为空不正确	
            /// 43101	用户拒绝接受消息，如果用户之前曾经订阅过，则表示用户取消了订阅关系	
            /// 47003	模板参数不准确，可能为空或者不满足规则，errmsg会提示具体是哪个字段出错	
            /// 41030	page路径不正确，需要保证在现网版本小程序中存在，与app.json保持一致
            /// 次数限制：开通支付能力的是3kw/日，没开通的是1kw/日
            /// </summary>
            public int errcode { get; set; }
            /// <summary>
            /// 错误信息
            /// </summary>
            public string errmsg { get; set; }
        }
    }
}
