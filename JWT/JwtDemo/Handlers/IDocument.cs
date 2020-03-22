using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtDemo.Handlers
{
    public interface IDocument
    {
        string Creator { get; set; }
    }
}
