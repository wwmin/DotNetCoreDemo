using FreeSql.DataAnnotations;
using System;

namespace BlazorAppApi
{
    /// <summary>
    /// 博客内容
    /// </summary>
    public class Blog : BaseEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Column(DbType = "varchar(50)")]
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Column(DbType = "varchar(500)")]
        public string Content { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateUserId { get; set; }
    }
}
