using System;
using System.Threading.Tasks;

namespace ConsoleAsksFor;

/// <summary>
/// Extension methods for <see cref="Task{T}"/> related to handling of <see cref="TaskCanceledByF12Exception"/>.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// When <see cref="TaskCanceledByF12Exception"/> is thrown returns <see cref="Task"/>&lt;<see cref="Nullable{T}" />&gt; with result=null.
    /// </summary>
    /// <param name="task"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T?> WithNullableWhenF12<T>(this Task<T> task)
        where T : struct
    {
        try
        {
            return await task;
        }
        catch (TaskCanceledByF12Exception)
        {
            return null;
        }
    }

    /// <summary>
    /// When <see cref="TaskCanceledByF12Exception"/> is thrown returns <see cref="Task{T}"/> with result null.
    /// </summary>
    /// <param name="task"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static async Task<T?> WithNullWhenF12<T>(this Task<T> task)
        where T : class
    {
        try
        {
            return await task;
        }
        catch (TaskCanceledByF12Exception)
        {
            return null;
        }
    }
}