using AutoMapper;
using freeSqlWeb1.Domain;
using freeSqlWeb1.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.AutoMappers
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<User, UserDto>().ForMember(e => e.Name, v => v.MapFrom(a => a.Name));
            CreateMap<UserDto, User>();// TODO: 自动设置默认时间 .ForMember(e => e.CreateTime, v => new DateTime());
        }
    }
}
