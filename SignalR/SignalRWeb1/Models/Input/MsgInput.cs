using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRWeb1.Models.Input
{
    public class MsgInput
    {
       

        /// <summary>
        /// to connection
        /// </summary>
        public class SendToConnectionIdMsgInput
        {
            /// <summary>
            ///to user id
            /// </summary>
            [Required]
            public string ConnectionId { get; set; }
            /// <summary>
            /// message
            /// </summary>
            [Required]
            public string Message { get; set; }
        }

        /// <summary>
        /// to group
        /// </summary>
        public class SendToGroupMsgInput
        {
            /// <summary>
            ///to user id
            /// </summary>
            [Required]
            public string GroupId { get; set; }
            /// <summary>
            /// message
            /// </summary>
            [Required]
            public string Message { get; set; }
        }
    }
}
