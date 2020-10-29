using static BlazorAppApi.Enums;

namespace BlazorAppApi.Models
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
