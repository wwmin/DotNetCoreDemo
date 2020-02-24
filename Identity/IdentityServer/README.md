dbfist

1.安装
- Microsoft.EntityFrameworkCore
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools (迁移数据库用)
2.创建实体(此为admin)
3.创建dbcontext
4.在appsettings.json中写入数据库连接串:
```
"ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=identity4;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
```
4.在startup.cs中的configService中引用
```
 services.AddDbContext<EFContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));//注入DbContext
```
5.创建service
```
namespace IdentityServer.Services
{
    public interface IAdminService
    {
        Task<Admin> GetByStr(string username, string pwd);
    }

    public class AdminService : IAdminService
    {
        public EFContext db;

        public AdminService(EFContext _efContext)
        {
            db = _efContext;
        }

        public async Task<Admin> GetByStr(string username, string pwd)
        {
            Admin a = await db.Admin.Where(a => a.UserName == username && a.Password == pwd).SingleOrDefaultAsync();
            return a;
        }
    }
}
```
6.将service注入到容器中,即在startup.cs中的configService中引用
```
#region 注入service
services.AddTransient<IAdminService, AdminService>();//service注入
#endregion
```
7.在程序包管理器控制台中使用命令行工具迁移数据库,注意程序包管理器默认项目应改为当前要迁移的项目
```
Add-Migration Init  //其中Init是你的版本名称
update-database Init //更新数据库操作 init为版本名称
```
8.修改model之后使用同样的命令更新

9.使用vscode控制台即cmd控制台命令更新数据库
参考:https://docs.microsoft.com/zh-cn/aspnet/core/data/ef-rp/migrations?view=aspnetcore-3.1&tabs=visual-studio
```
//安装EF cli
dotnet tool install --global dotnet-ef
dotnet ef migrations add InitialCreate
dotnet ef database update
```