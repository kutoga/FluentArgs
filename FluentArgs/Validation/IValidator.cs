namespace FluentArgs.Validation
{
    public interface IValidator<T>
    {
        bool IsValid(T value, out string? errorMessage);
    }
}
