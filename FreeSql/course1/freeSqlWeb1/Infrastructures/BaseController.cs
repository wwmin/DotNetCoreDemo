using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.Infrastructures
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected readonly IFreeSql _freesql;
        protected readonly IMapper _mapper;
        public BaseController(IFreeSql sql = null, IMapper mapper = null)
        {
            _freesql = sql;
            _mapper = mapper;
        }
    }
}
