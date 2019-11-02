# TestDemo
测试各种库和解决方案,为自己和他人留下可行方案

:sparkling_heart: 欢迎一起完善,补充,修正,提意见

已包含demo:
### RabbitMQ官方tutorial
- t1:
- t2:
- t3:
- t4:
- t5:
- t6:

### .net core 3.x webapi中Client的使用方式
使用polly熔断重试方式解决调用失败的重试方案

### .net core 3.x 使用xUnit测试webapi
配置了config,以及在controller中使用config,
注意:在startup中将config注入了
另: 在开发过程中使用xunit可以做到持续集成测试,配合dotnet watch test,直接监视xunit的项目,则在testApi中的任何修改都能触发此构建
也可以配合dotnet watch run做到监视testApi项目使用postMan自己测试
