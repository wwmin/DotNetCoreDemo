using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static freeSqlWeb1.Config.Enums;

namespace freeSqlWeb1.DTO
{
    public class UserDto
    {
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public Gender Gender { get; set; }

        /// <summary>
        /// age
        /// </summary>
        public int Age { get; set; }
    }
}
