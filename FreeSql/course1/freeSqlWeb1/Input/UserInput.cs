using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.Input
{
    public class UserInput
    {
        /// <summary>
        /// name
        /// </summary>
        [Required]
        [StringLength(maximumLength: 50)]
        public string Name { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [Required]
        public DateTime Birthday { get; set; }
    }
}
