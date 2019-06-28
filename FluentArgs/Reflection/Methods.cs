namespace FluentArgs.Reflection
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class Methods
    {
        internal static object? Invoke(object targetMethod, IEnumerable<object> arguments)
        {
            return targetMethod.GetType().GetMethod("Invoke").Invoke(targetMethod, arguments.ToArray());
        }
    }
}
