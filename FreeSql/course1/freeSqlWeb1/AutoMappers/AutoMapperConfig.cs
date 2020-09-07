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
            CreateMap<User, UserDto>().ForMember(c => c.Name, opt => opt.MapFrom(o => o.Name)).ForMember(c => c.Age, opt => opt.MapFrom(src => src.Birthday.Year > 1970 ? DateTime.Now.Year - src.Birthday.Year : -1));
            //CreateMap<UserDto, User>();// TODO: 自动设置默认时间 .ForMember(e => e.CreateTime, v => new DateTime());
            CreateMap<UserDto, User>().ForMember(c => c.CreateTime, opt => opt.MapFrom(o => DateTime.Now))
                .ForMember(c => c.Birthday, opt => opt.MapFrom(src => src.Age > 0 ? DateTime.Today.AddYears(-src.Age) : new DateTime(2000, 1, 1)));
        }
    }
}
