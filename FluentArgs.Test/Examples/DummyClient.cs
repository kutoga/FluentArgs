namespace FluentArgs.Test.Examples
{
    using System.Collections.Immutable;

    public class DummyClient
    {
        public IImmutableList<(string source, string target)> CopyFileCalls { get; private set; } = ImmutableList<(string, string)>.Empty;

        public IImmutableList<string> DeleteFileCalls { get; private set; } = ImmutableList<string>.Empty;

        public IImmutableList<int?> ResetAccountCalls { get; private set; } = ImmutableList<int?>.Empty;

        public string ApiKey { get; set; }

        public void CopyFile(string source, string target)
        {
            CopyFileCalls = CopyFileCalls.Add((source, target));
        }

        public void DeleteFile(string file)
        {
            DeleteFileCalls = DeleteFileCalls.Add(file);
        }

        public void ResetAccount(int? timeout)
        {
            ResetAccountCalls = ResetAccountCalls.Add(timeout);
        }
    }
}
