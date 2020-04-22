using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace CSCommon.Dynamic
{
    public class Duck : DynamicObject
    {
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            //return base.TryInvokeMember(binder, args, out result);
            Console.WriteLine(binder.Name + " Method was called.");
            result = null;
            return true;
        }
    }
}
