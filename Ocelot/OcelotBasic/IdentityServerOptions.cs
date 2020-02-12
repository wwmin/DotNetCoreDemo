﻿namespace OcelotBasic
{
    /// <summary>
    /// IdentityServer的配置选项
    /// </summary>
    public class IdentityServerOptions
    {
        /// <summary>
        /// 授权服务器的IP地址
        /// </summary>
        public string ServerIP { get; set; }
        /// <summary>
        /// 授权服务器的端口号
        /// </summary>
        public int ServerPort { get; set; }
        /// <summary>
        /// access_token的类型,获取access_token的时候返回参数中的token_type一致
        /// </summary>
        public string IdentityScheme { get; set; }
        /// <summary>
        /// 资源名称,认证服务注册的资源列表名称一致
        /// </summary>
        public string ResourceName { get; set; }
    }
}