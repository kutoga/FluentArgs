namespace FluentArgs.Extensions
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal static class IEnumerableExtensions
    {
        public static async Task Serialize(this IEnumerable<Task> tasks)
        {
            foreach (var task in tasks)
            {
                await task.ConfigureAwait(false);
            }
        }
    }
}
