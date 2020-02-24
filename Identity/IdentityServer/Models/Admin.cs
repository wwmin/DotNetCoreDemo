using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        [MaxLength(16)]
        [Required]
        public string UserName { get; set; }
        [Required]
        [MaxLength(32)]
        public string Password { get; set; }
        [MaxLength(255)]
        public string Remark { get; set; }
    }
}
