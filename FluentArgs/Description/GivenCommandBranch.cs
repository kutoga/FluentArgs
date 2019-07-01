using System;
using System.Collections.Generic;
using System.Text;

namespace FluentArgs.Description
{
    internal class GivenCommandBranch
    {
        public GivenCommandBranchType Type { get; }

        public object RequiredValue { get; }

        public Type ValueType { get; }

        public Func<string, object>? Parser { get; set; }
    }
}
