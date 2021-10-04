namespace ConsoleAsksFor.Sdk
{
    /// <summary>
    /// Hook to let ConsoleAsksFor libraries set static state during startup.
    /// </summary>
    public interface IOnStartupHook
    {
        /// <summary>
        /// Hook to let ConsoleAsksFor libraries set static state during startup.
        /// </summary>
        void Initialize();
    }
}