using System.Collections.Generic;

using ConsoleAsksFor.TestUtils;

using FluentAssertions;

using Xunit;

namespace ConsoleAsksFor.Tests
{
    public class ItemQuestionIntellisenseTests
    {
        private readonly ItemQuestionIntellisense _sut = new(QuestionItems.CreateWithEscapedSplitter(new[] { "A1", "Bb1", "Bb3", "bb2", "Cc" }));

        private static readonly IntellisenseUseCases UseCases = new()
        {
            FromUser = new List<IntellisenseUseCases.FromUserInput>
            {
                new() { Input = "",     Previous = "Cc",  Next = "A1",  UseCase = "No value" },
                new() { Input = "Q",    Previous = null,  Next = null,  UseCase = "Not matchable value" },
                new() { Input = "Bb1",  Previous = null, Next = null, UseCase = "Complete value" },
                new() { Input = "Bb",   Previous = "bb2", Next = "Bb1", UseCase = "Invalid value" },
            },
            FromIntellisense = new List<IntellisenseUseCases.FromIntellisenseInput>
            {
                new() { Input = "Bb1", Hint = "Bb", Previous = "bb2", Next = "Bb3", UseCase = "Valid first value" },
                new() { Input = "Bb3", Hint = "Bb", Previous = "Bb1", Next = "bb2", UseCase = "Valid middle value" },
                new() { Input = "bb2", Hint = "Bb", Previous = "Bb3", Next = "Bb1", UseCase = "Valid last value" },
            },
        };

        public static TheoryData<string, string, string?> CompleteValueUseCases => UseCases.CompleteValueUseCases;

        public static TheoryData<string, string, string, string?> PreviousValueUseCases => UseCases.PreviousValueUseCases;

        public static TheoryData<string, string, string, string?> NextValueUseCases => UseCases.NextValueUseCases;

        [Theory]
        [MemberData(nameof(CompleteValueUseCases))]
        public void CompleteValue_Returns_CorrectValue(string useCase, string value, string? newValue)
        {
            _sut.CompleteValue(value).Should().Be(newValue, useCase);
        }

        [Theory]
        [MemberData(nameof(PreviousValueUseCases))]
        public void PreviousValue_Returns_CorrectValue(string useCase, string value, string hint, string? newValue)
        {
            _sut.PreviousValue(value, hint).Should().Be(newValue, useCase);
        }

        [Theory]
        [MemberData(nameof(NextValueUseCases))]
        public void NextValue_Returns_CorrectValue(string useCase, string value, string hint, string? newValue)
        {
            _sut.NextValue(value, hint).Should().Be(newValue, useCase);
        }
    }
}