using FreeSql.DataAnnotations;

namespace BlazorAppApi
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
