using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using WeChatService.Infrastructure;
using WeChatService.Model;

namespace WeChatService
{
    public class WeChatService
    {
        /// <summary>
        /// 获取小程序全局唯一后台接口调用凭证(access_token)
        /// 调用绝大多数后台接口时都需使用 access_token，开发者需要进行妥善保存。
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public Result GetAccessToken(string appId, string secret)
        {
            Result rs = new Result();
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(secret))
            {
                rs.success = false;
                rs.message = "appId,secret不能为空";
                return rs;
            }


            using (var client = Helper.CreateHttpClient(Const.WECHAT.URL_ROOT))
            {
                var url = string.Format(Const.WECHAT.URL_PUBLIC_TOKEN, appId, secret);
                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var statusCode = response.StatusCode;
                if (statusCode != HttpStatusCode.OK)
                {
                    rs.success = false;
                    rs.message = content;
                    return rs;
                }
                WeChatModel.AccessTokenResult o = Helper.DeserializeJsonToEntity<WeChatModel.AccessTokenResult>(Helper.Unicode2String(content));
                if (o == null || o.errcode != 0)
                {
                    rs.success = false;
                    rs.message = o.errmsg;
                    return rs;
                }
                rs.data = o;
                return rs;
            }
        }

        /// <summary>
        /// 根据code获取CodeToken
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="secret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public Result GetAccessWeChatCodeToken(string appId, string secret, string code)
        {
            Result rs = new Result();
            if (string.IsNullOrWhiteSpace(appId) || string.IsNullOrWhiteSpace(secret) || string.IsNullOrWhiteSpace(code))
            {
                rs.success = false;
                rs.message = "appId ,secret 及 code不能为空.";
                return rs;
            }
            using (var client = Helper.CreateHttpClient(Const.WECHAT.URL_ROOT))
            {
                var url = string.Format(Const.WECHAT.URL_CODE_TOKEN, appId, secret, code);
                var response = client.GetAsync(url).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var statusCode = response.StatusCode;
                if (statusCode != HttpStatusCode.OK)
                {
                    rs.success = false;
                    rs.message = content;
                    return rs;
                }
                WeChatModel.AccessWeChatCodeTokenResult o = Helper.DeserializeJsonToEntity<WeChatModel.AccessWeChatCodeTokenResult>(Helper.Unicode2String(content));
                if (o == null || o.errcode != 0)
                {
                    rs.success = false;
                    rs.message = o.errmsg;
                    rs.data = o;
                    return rs;
                }
                rs.success = true;
                rs.data = o;
                return rs;
            }
        }

        /// <summary>
        /// 微信服务端接口签名验证
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        public Result CheckSignature(WeChatModel.SignatureConditionModel condition)
        {
            Result rs = new Result();

            if (string.IsNullOrWhiteSpace(condition.echostr) || string.IsNullOrWhiteSpace(condition.signature) || string.IsNullOrWhiteSpace(condition.timestamp)
                || string.IsNullOrWhiteSpace(condition.nonce))
            {
                rs.success = false;
                rs.message = $"参数错误,请检查参数{nameof(condition.echostr)},{nameof(condition.signature)},{nameof(condition.timestamp)},{nameof(condition.nonce)},不能为空";
                return rs;
            }
            if (!CheckSignature(condition.token, condition.signature, condition.timestamp, condition.nonce))
            {
                rs.success = false;
                rs.message = "签名验证错误";
                return rs;
            }
            return rs;
        }
        /// <summary>
        /// 验证微信签名
        /// </summary>
        /// <param name="token"></param>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        private bool CheckSignature(string token, string signature, string timestamp, string nonce)
        {
            //string[] parameter = { echostr, timestamp, nonce };
            //Array.Sort(parameter);
            var parameter = new List<string> { token, timestamp, nonce };
            parameter.Sort();
            string tmpStr = string.Join("", parameter);
            var data = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(tmpStr));
            var sb = new StringBuilder();
            foreach (var d in data)
            {
                sb.Append(d.ToString("X2"));
            }
            tmpStr = sb.ToString();
            tmpStr = tmpStr.ToLower();
            return tmpStr == signature;
        }

        /// <summary>
        /// 小程序模板消息推送 (官方提示已废弃,清使用订阅消息推送)
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Result TemplateMessageSend(WeChatModel.MessageSendCondition param)
        {
            Result rs = new Result();
            if (param == null || string.IsNullOrWhiteSpace(param.access_token) || string.IsNullOrWhiteSpace(param.touser) || string.IsNullOrWhiteSpace(param.template_id) || string.IsNullOrWhiteSpace(param.form_id))
            {
                rs.success = false;
                rs.message = $"参数错误,请将必填项填写完整,{nameof(param.access_token)},{nameof(param.touser)},{nameof(param.template_id)},{nameof(param.form_id)}";
                return rs;
            }
            using (var client = Helper.CreateHttpClient(Const.WECHAT.URL_ROOT))
            {
                var url = string.Format(Const.WECHAT.URL_TEMPLATE_MESSAGE_SEND, param.access_token);
                var httpContent = Helper.SerializeObject(param);
                var requestContent = new StringContent(httpContent);

                var response = client.PostAsync(url, requestContent).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var statusCode = response.StatusCode;
                if (statusCode != HttpStatusCode.OK)
                {
                    rs.success = false;
                    rs.message = content;
                    return rs;
                }
                WeChatModel.MessageSendResult o = Helper.DeserializeJsonToEntity<WeChatModel.MessageSendResult>(Helper.Unicode2String(content));
                if (o == null || o.errcode != 0)
                {
                    rs.success = false;
                    rs.message = o.errmsg;
                    rs.data = o;
                    return rs;
                }
                rs.data = o;
                return rs;
            }
        }

        /// <summary>
        /// 小程序订阅消息推送
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public Result SubscribeMessageSend(WeChatModel.SubscribeMessageSendCondition param)
        {
            Result rs = new Result();
            if (param == null || string.IsNullOrWhiteSpace(param.access_token) || string.IsNullOrWhiteSpace(param.touser) || string.IsNullOrWhiteSpace(param.template_id))
            {
                rs.success = false;
                rs.message = $"参数错误,请将必填项填写完整,{nameof(param.access_token)},{nameof(param.touser)},{nameof(param.template_id)}";
                return rs;
            }
            using (var client = Helper.CreateHttpClient(Const.WECHAT.URL_ROOT))
            {
                var url = string.Format(Const.WECHAT.URL_SUBSCRIBE_MESSAGE_SEND, param.access_token);
                var httpContent = Helper.SerializeObject(param);
                var requestContent = new StringContent(httpContent);

                var response = client.PostAsync(url, requestContent).Result;
                var content = response.Content.ReadAsStringAsync().Result;
                var statusCode = response.StatusCode;
                if (statusCode != HttpStatusCode.OK)
                {
                    rs.success = false;
                    rs.message = content;
                    return rs;
                }
                WeChatModel.SubscribeMessageSendResult o = Helper.DeserializeJsonToEntity<WeChatModel.SubscribeMessageSendResult>(Helper.Unicode2String(content));
                if (o == null || o.errcode != 0)
                {
                    rs.success = false;
                    rs.message = o.errmsg;
                    rs.data = o;
                    return rs;
                }
                rs.data = o;
                return rs;
            }
        }
    }
}
