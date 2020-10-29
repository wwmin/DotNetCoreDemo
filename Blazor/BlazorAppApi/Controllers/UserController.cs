using AutoMapper;
using BlazorAppApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorAppApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        protected readonly IFreeSql _sql;
        protected readonly IMapper _mapper;
        public UserController(IFreeSql freeSql, IMapper mapper)
        {
            _sql = freeSql;
            _mapper = mapper;
        }

        /// <summary>
        /// 查询所有user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDto>>> Get()
        {

            List<User> users = await _sql.Select<User>().OrderByDescending(r => r.Name).ToListAsync();
            var userDtoList = _mapper.Map<List<User>, List<UserDto>>(users);
            return userDtoList;
        }

        /// <summary>
        /// 查询所有user name
        /// </summary>
        /// <returns></returns>
        [HttpGet("allName")]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<string>>> GetAllName()
        {
            List<string> users = await _sql.Select<User>().OrderByDescending(r => r.Name).ToListAsync(x => x.Name);

            return users;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public async Task<ActionResult> AddUser([FromBody] UserDto userDto)
        {
            //var user = AutoMapperHelper.MapTo<UserDto,User>(userDto);
            var user = _mapper.Map<UserDto, User>(userDto);
            var r =await _sql.Insert<User>(user).ExecuteIdentityAsync();
            return Ok(r);
        }

        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("edit/{id}")]
        public async Task<ActionResult> EditUser([FromBody] UserDto userDto, int id)
        {
            var user = await _sql.Select<User>(id).FirstAsync();
            user.Name = userDto.Name;
            user.Gender = userDto.Gender;
            var i = _sql.Update<User>(user);
            return Ok(i);
        }

    }
}
