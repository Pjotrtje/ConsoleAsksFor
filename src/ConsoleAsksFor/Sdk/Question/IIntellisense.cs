namespace ConsoleAsksFor.Sdk
{
    /// <summary>
    /// Tab (with/without Ctrl/Shift) and Ctrl+Space behaviour for implementations of <see cref="IQuestion{TAnswer}"/>.
    /// </summary>
    public interface IIntellisense
    {
        /// <summary>
        /// Complete/reformat value if possible.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Completed/reformated value when success; null when no success.</returns>
        string? CompleteValue(string value);

        /// <summary>
        /// Complete or reformat value if possible. When already complete try to get previous value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hint"></param>
        /// <returns>Completed/previous value when success; null when no success.</returns>
        string? PreviousValue(string value, string hint);

        /// <summary>
        /// Complete or reformat value if possible. When already complete try to get next value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hint"></param>
        /// <returns>Completed/next value when success; null when no success.</returns>
        string? NextValue(string value, string hint);
    }
}