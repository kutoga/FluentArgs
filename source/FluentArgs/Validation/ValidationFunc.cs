namespace FluentArgs.Validation
{
    public delegate bool ValidationFunc<in T>(T value, out string? errorMessage);

    internal static class ValidationFuncExtensions
    {
        public static ValidationFunc<object> ToObjectInput<T>(this ValidationFunc<T> validation)
        {
             return (object value, out string? errorMessage) => validation((T)value, out errorMessage);
        }
    }
}
