using System;
using System.Collections.Generic;

using Xunit;

namespace ConsoleAsksFor.TestUtils
{
    public sealed record IntellisenseUseCases
    {
        public sealed record FromUserInput
        {
            public string Input { get; init; } = null!;
            public string? Previous { get; init; }
            public string? Complete { get; init; }
            public string? Next { get; init; }
            public string UseCase { get; init; } = null!;

            public string FullUseCaseCase => $"{UseCase} (UserInput)";
        }

        public sealed record FromIntellisenseInput
        {
            public string Input { get; init; } = null!;
            public string Hint { get; init; } = null!;
            public string? Previous { get; init; }
            public string? Next { get; init; }
            public string UseCase { get; init; } = null!;

            public string FullUseCaseCase => $"{UseCase} (Intellisense)";
        }

        public IReadOnlyCollection<FromUserInput> FromUser { get; init; } = Array.Empty<FromUserInput>();
        public IReadOnlyCollection<FromIntellisenseInput> FromIntellisense { get; init; } = Array.Empty<FromIntellisenseInput>();

        public TheoryData<string, string, string?> CompleteValueUseCases
        {
            get
            {
                var data = new TheoryData<string, string, string?>();
                foreach (var item in FromUser)
                {
                    data.Add(item.FullUseCaseCase, item.Input, item.Complete ?? item.Next);
                }
                return data;
            }
        }

        public TheoryData<string, string, string, string?> PreviousValueUseCases
        {
            get
            {
                var data = new TheoryData<string, string, string, string?>();
                foreach (var item in FromIntellisense)
                {
                    data.Add(item.FullUseCaseCase, item.Input, item.Hint, item.Previous);
                }

                foreach (var item in FromUser)
                {
                    data.Add(item.FullUseCaseCase, item.Input, item.Input, item.Previous);
                }
                return data;
            }
        }

        public TheoryData<string, string, string, string?> NextValueUseCases
        {
            get
            {
                var data = new TheoryData<string, string, string, string?>();
                foreach (var item in FromIntellisense)
                {
                    data.Add(item.FullUseCaseCase, item.Input, item.Hint, item.Next);
                }

                foreach (var item in FromUser)
                {
                    data.Add(item.FullUseCaseCase, item.Input, item.Input, item.Next);
                }
                return data;
            }
        }
    }
}