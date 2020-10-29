using FreeSql.DataAnnotations;
using System;
using static BlazorAppApi.Enums;

namespace BlazorAppApi
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : BaseEntity
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Column(DbType = "varchar(50)", IsNullable = false)]
        public string Name { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
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
