using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppApi
{
    /// <summary>
    /// 视频
    /// </summary>
    public class Video : BaseEntity
    {
        public string biz { get; set; }
        public string appmsgid { get; set; }
        public int idx { get; set; }
        public string vid { get; set; }
        public string title { get; set; }
        public string digest { get; set; }
        public string cover_url { get; set; }
        public int duration { get; set; }
        public int publish_time { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string page_url { get; set; }
        public int like_num { get; set; }
        public int read_num { get; set; }
    }
}
