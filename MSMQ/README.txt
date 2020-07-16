- 说明：
MSMQ工程是 .NET Framework v4.7.2 的项目
因为需要开启MSMQ服务，只支持windows系统
内部引用System.Messaging
外部nuget包 Quartz.Net

- dotnet core项目推荐使用RabbitMQ，dotnetcore 目前不支持MSMQ。