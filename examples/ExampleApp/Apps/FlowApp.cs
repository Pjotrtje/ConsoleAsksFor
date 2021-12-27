using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

using ConsoleAsksFor;

namespace ExampleApp.Apps;

internal static class FlowStep
{
    public static StructFlowStep<T> Create<T>(
        Func<T?, Task<T>> valueGetter,
        T? initialDefaultValue = null) where T : struct
        => new(initialDefaultValue, null, valueGetter);

    public static ClassFlowStep<T> Create<T>(
        Func<T?, Task<T>> valueGetter,
        T? initialDefaultValue = null) where T : class
        => new(initialDefaultValue, null, valueGetter);
}

internal sealed class ClassFlowStep<T>
    where T : class
{
    private readonly Func<T?, Task<T>> _valueGetter;
    private readonly T? _defaultValue;

    [Pure]
    public T? Answer { get; }

    [Pure]
    public ClassFlowStep<T> WithNoValue()
    {
        return new(_defaultValue, null, _valueGetter);
    }

    [Pure]
    [MemberNotNullWhen(false, nameof(Answer))]
    public bool IsCancelled => Answer is null;

    [Pure]
    public async Task<ClassFlowStep<T>> WithReAskedAnswerIfNeeded()
    {
        if (Answer is not null)
        {
            return this;
        }

        var newAnswer = await _valueGetter(_defaultValue).WithNullWhenF12();
        var newDefaultValue = newAnswer ?? _defaultValue;
        return new(newDefaultValue, newAnswer, _valueGetter);
    }

    public ClassFlowStep(T? initialDefaultValue, T? answer, Func<T?, Task<T>> valueGetter)
    {
        Answer = answer;
        _valueGetter = valueGetter;
        _defaultValue = initialDefaultValue;
    }
}

internal sealed class StructFlowStep<T>
    where T : struct
{
    private readonly Func<T?, Task<T>> _valueGetter;
    private readonly T? _defaultValue;

    [Pure]
    public T? Answer { get; }

    [Pure]
    public StructFlowStep<T> WithNoValue()
    {
        return new(_defaultValue, null, _valueGetter);
    }

    [Pure]
    [MemberNotNullWhen(false, nameof(Answer))]
    public bool IsCancelled => Answer is null;

    [Pure]
    public async Task<StructFlowStep<T>> WithReAskedAnswerIfNeeded()
    {
        if (Answer is not null)
        {
            return this;
        }

        var newAnswer = await _valueGetter(_defaultValue).WithNullableWhenF12();
        var newDefaultValue = newAnswer ?? _defaultValue;
        return new(newDefaultValue, newAnswer, _valueGetter);
    }

    public StructFlowStep(T? initialDefaultValue, T? answer, Func<T?, Task<T>> valueGetter)
    {
        Answer = answer;
        _valueGetter = valueGetter;
        _defaultValue = initialDefaultValue;
    }
}

internal sealed class FlowApp : IApp
{
    private readonly IConsole _console;

    public FlowApp(IConsole console)
    {
        _console = console;
    }

    public async Task Run()
    {
        var t = await Get();
        if (t is not null)
        {
            var x = t.Value;
            _console.WriteSuccessLine($"{x.FirstName} - {x.LastName} - {x.Age}");
        }
        else
        {
            _console.WriteWarningLine("F12!");
        }
    }

    private async Task<(string FirstName, string LastName, int Age, decimal Lenght)?> Get()
    {
        var firstNameStep = FlowStep.Create<string>(
            dv => _console.AskForString("What is your first name?", dv));

        var lastNameStep = FlowStep.Create<string>(
            dv => _console.AskForString("What is your last name?", dv));

        var ageStep = FlowStep.Create<int>(
            dv => _console.AskForInt("What is your age?", defaultValue: dv));

        var lengthStep = FlowStep.Create<decimal>(
            dv => _console.AskForDecimal("What is your Length?", Scale.Two, defaultValue: dv));

        while (true)
        {
            firstNameStep = await firstNameStep.WithReAskedAnswerIfNeeded();
            if (firstNameStep.IsCancelled)
            {
                return null;
            }

            lastNameStep = await lastNameStep.WithReAskedAnswerIfNeeded();
            if (lastNameStep.IsCancelled)
            {
                firstNameStep = firstNameStep.WithNoValue();
                continue;
            }

            ageStep = await ageStep.WithReAskedAnswerIfNeeded();
            if (ageStep.IsCancelled)
            {
                lastNameStep = lastNameStep.WithNoValue();
                continue;
            }

            lengthStep = await lengthStep.WithReAskedAnswerIfNeeded();
            if (lengthStep.IsCancelled)
            {
                ageStep = ageStep.WithNoValue();
                continue;
            }

            return (firstNameStep.Answer, lastNameStep.Answer, ageStep.Answer.Value, lengthStep.Answer.Value);
        }
    }
}