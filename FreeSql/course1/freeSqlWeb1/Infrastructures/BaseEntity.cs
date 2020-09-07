using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace freeSqlWeb1.Infrastructures
{
    /// <summary>
    /// 实体类接口
    /// </summary>
    public class BaseEntity
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int Id { get; set; }
    }
}
