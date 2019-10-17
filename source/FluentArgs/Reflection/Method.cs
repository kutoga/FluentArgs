namespace FluentArgs.Reflection
{
    using System.Collections.Generic;
    using System.Linq;

    internal static class Method
    {
        internal static object? InvokeWrappedMethod(object targetMethod, IEnumerable<object> arguments, bool invokeAtleastOnce)
        {
            var currentValue = targetMethod;
            var reversedArguments = arguments.Reverse().ToArray();

            if (reversedArguments.Length > 0)
            {
                foreach (var argument in arguments.Reverse())
                {
                    currentValue = InvokeMethod(currentValue, new[] { argument });
                }
            }
            else if (invokeAtleastOnce)
            {
                currentValue = InvokeMethod(currentValue, Enumerable.Empty<object>());
            }

            return currentValue;
        }

        private static object InvokeMethod(object targetMethod, IEnumerable<object> arguments)
        {
            return targetMethod.GetType().GetMethod("Invoke").Invoke(targetMethod, arguments.ToArray());
        }
    }
}
