using FreeSql.DataAnnotations;
using System;

namespace freeSqlWeb1.Domain
{
    public class Post
    {
        [Column(IsIdentity = true, IsPrimary = true)]
        public int PostId { get; set; }
        /// <summary>
        /// 发布内容
        /// </summary>
        [Column(DbType = "varchar(50)")]
        public string ReplyContent { get; set; }
        /// <summary>
        /// 文章ID
        /// </summary>
        public int BlogId { get; set; }
        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime ReplyTime { get; set; }
        /// <summary>
        /// 文章导航属性
        /// </summary>
        public virtual Blog Blog { get; set; }
    }
}
