namespace FluentArgs.Description
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    internal class GivenParameter
    {
        private GivenParameter(Name name, Type type, bool requireExactValue, object? requiredValue, Func<string, object>? parser)
        {
            Name = name;
            Type = type;
            RequireExactValue = requireExactValue;
            RequiredValue = requiredValue;
            Parser = parser;
        }

        public Name Name { get; }

        public Type Type { get; }

        public bool RequireExactValue { get; }

        public object? RequiredValue { get; }

        public Func<string, object>? Parser { get; set; }

        public static GivenParameter Exists(Name name)
        {
            return new GivenParameter(name, typeof(string), false, default, default);
        }

        public static GivenParameter HasValue(Name name, Type type, object requiredValue, Func<string, object>? parser = null)
        {
            return new GivenParameter(name, type, true, requiredValue, parser);
        }
    }
}
