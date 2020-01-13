using DotNetCore.CAP.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapWeb1.Infrastructure
{
    public class TestAuthorizationFilter:IDashboardAuthorizationFilter
    {
        public async Task<bool> AuthorizeAsync(DashboardContext context)
        {
            if (context.Request.GetQuery("bearer") == "myauth")
            {
                return true;
            }
            return true;//false
        }
    }
}
