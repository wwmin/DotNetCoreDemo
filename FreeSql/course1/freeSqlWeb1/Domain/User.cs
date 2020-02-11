using FreeSql.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static freeSqlWeb1.Config.Enums;

namespace freeSqlWeb1.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户guid
        /// </summary>
        [Column(IsIdentity = true, IsPrimary = true)]
        public Guid Id { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [Column(DbType = "varchar(50)", IsNullable = false)]
        public string Name { get; set; }
        /// <summary>
        /// 生日
        /// </summary
        public DateTimeOffset Birthday { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [Column(DbType = "int")]
        public Gender Gender { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

    }
}
