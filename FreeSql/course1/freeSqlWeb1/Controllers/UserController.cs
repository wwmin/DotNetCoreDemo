using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using freeSqlWeb1.AutoMappers;
using freeSqlWeb1.Domain;
using freeSqlWeb1.DTO;
using freeSqlWeb1.Infrastructures;
using freeSqlWeb1.Infrastructures.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace freeSqlWeb1.Controllers
{

    public class UserController : BaseController
    {
        public UserController(IFreeSql freesql, IMapper mapper) : base(sql: freesql, mapper: mapper)
        {
        }
        /// <summary>
        /// 查询所有user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {

            string us = await RedisUtil.StringGetAsync("users");
            if (us == null)
            {
                await RedisUtil.StringSetAsync("users", "users");
            }
            
            List<User> users = _freesql.Select<User>().OrderByDescending(r => r.Name).ToList();
            var userDtoList = _mapper.Map<List<User>, List<UserDto>>(users);
            return userDtoList;
        }

        /// <summary>
        /// 查询所有user name
        /// </summary>
        /// <returns></returns>
        [HttpGet("allName")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<string>> GetAllName()
        {
            List<string> users = _freesql.Select<User>().OrderByDescending(r => r.Name).ToList(x => x.Name);

            return users;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddUser([FromBody] UserDto userDto)
        {
            //var user = AutoMapperHelper.MapTo<UserDto,User>(userDto);
            var user = _mapper.Map<UserDto, User>(userDto);
            var r = _freesql.Insert<User>(user).ExecuteIdentity();
            return Ok(r);
        }

    }

}