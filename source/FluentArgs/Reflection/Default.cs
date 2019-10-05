namespace FluentArgs.Reflection
{
    using System;

    internal static class Default
    {
        public static object? Instance(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }

            return null;
        }
    }
}
