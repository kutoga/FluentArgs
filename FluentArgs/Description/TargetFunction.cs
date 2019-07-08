namespace FluentArgs.Description
{
    internal class TargetFunction
    {
        public TargetFunction(object target, bool callWithAdditionalArgs)
        {
            Target = target;
            CallWithAdditionalArgs = callWithAdditionalArgs; //TODO: bad naming
        }

        public object Target { get; }

        public bool CallWithAdditionalArgs { get; }
    }
}
