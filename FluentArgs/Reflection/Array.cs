namespace FluentArgs.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal static class Array
    {
        public static object Create(Type type, IReadOnlyList<object> objects)
        {
            var arr = System.Array.CreateInstance(type, objects.Count);
            foreach (var (obj, i) in objects.Select((o, i) => (o, i)))
            {
                arr.SetValue(obj, i);
            }

            return arr;
        }
    }
}
