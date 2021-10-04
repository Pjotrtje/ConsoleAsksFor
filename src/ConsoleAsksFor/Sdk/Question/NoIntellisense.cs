namespace ConsoleAsksFor.Sdk
{
    /// <summary>
    /// Dummy implementation of <see cref="IIntellisense"/> which never returns any new value.
    /// </summary>
    public sealed class NoIntellisense : IIntellisense
    {
        /// <summary>
        /// <inheritdoc cref="IIntellisense.CompleteValue"/><br/>
        /// Dummy implementation, will always return null.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string? CompleteValue(string value) => null;

        /// <summary>
        /// <inheritdoc cref="IIntellisense.PreviousValue"/><br/>
        /// Dummy implementation, will always return null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hint"></param>
        /// <returns></returns>
        public string? PreviousValue(string value, string hint) => null;

        /// <summary>
        /// <inheritdoc cref="IIntellisense.NextValue"/><br/>
        /// Dummy implementation, will always return null.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hint"></param>
        /// <returns></returns>
        public string? NextValue(string value, string hint) => null;
    }
}
