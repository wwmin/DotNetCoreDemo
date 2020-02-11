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
        public Gender Gender { get; set; }
    }
}
