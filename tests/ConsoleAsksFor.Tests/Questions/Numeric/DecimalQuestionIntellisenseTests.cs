namespace ConsoleAsksFor.Tests;

public class DecimalQuestionIntellisenseTests
{
    public class Scale0
    {
        private readonly DecimalQuestionIntellisense _sut = new(
            new DecimalQuestionParser(
                new DecimalFormat(Scale.Zero),
                RangeConstraint.Between(900m, 5000m)),
            new DecimalFormat(Scale.Zero));

        private static readonly IntellisenseUseCases UseCases = new()
        {
            FromUser = new List<IntellisenseUseCases.FromUserInput>
            {
                new() { Input = "",      Previous = "5,000", Next = "900",   UseCase = "No Value" },
                new() { Input = " ",     Previous = "5,000", Next = "900",   UseCase = "Only whitespace" },
                new() { Input = "3",     Previous = "3,999", Next = "3,000", UseCase = "1 digit" },
                new() { Input = " 3 ",   Previous = "3,999", Next = "3,000", UseCase = "1 digit with whitespace" },
                new() { Input = "30",    Previous = "3,099", Next = "3,000", UseCase = "2 digits" },
                new() { Input = "300",   Previous = "3,009", Next = "3,000", UseCase = "3 digits" },
                new() { Input = "3000",  Previous = "3,000", Next = "3,000", UseCase = "4 digits - valid value - unformatted" },
                new() { Input = "3,000", Previous = null,    Next = null,    UseCase = "4 digits - valid value - formatted" },
                new() { Input = "8",     Previous = null,    Next = null,    UseCase = "Incomplete - never in range" },
                new() { Input = "0",     Previous = null,    Next = null,    UseCase = "Zero - never in range" },
                new() { Input = "lala",  Previous = null,    Next = null,    UseCase = "Invalid value" },
            },
            FromIntellisense = new List<IntellisenseUseCases.FromIntellisenseInput>
            {
                new() { Input = "3,000",   Hint = "30",   Previous = "3,099", Next = "3,001", UseCase = "Min value" },
                new() { Input = "3,050",   Hint = "30",   Previous = "3,049", Next = "3,051", UseCase = "Regular" },
                new() { Input = "3,050",   Hint = " 30 ", Previous = "3,049", Next = "3,051", UseCase = "Regular with whitespace around hint" },
                new() { Input = " 3,050 ", Hint = "30",   Previous = "3,049", Next = "3,051", UseCase = "Regular with whitespace around input" },
                new() { Input = "3,099",   Hint = "30",   Previous = "3,098", Next = "3,000", UseCase = "Max value" },
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

    public class Scale0EdgeCases
    {
        [Fact]
        public void Handle_0_In_Range()
        {
            var sut = new DecimalQuestionIntellisense(
                new DecimalQuestionParser(
                    new DecimalFormat(Scale.Zero),
                    RangeConstraint.Between(-100m, 100m)),
                new DecimalFormat(Scale.Zero));

            using var _ = new AssertionScope();
            sut.CompleteValue("0").Should().BeNull("because 0 cannot be completed to a valid value (ignoring leading 0's)");
            sut.PreviousValue("0", "0").Should().BeNull("because no values after 0 lead to valid number (ignoring leading 0's)");
            sut.NextValue("0", "0").Should().BeNull("because no values after 0 lead to valid number (ignoring leading 0's)");
            sut.PreviousValue("0", "").Should().Be("-1");
            sut.NextValue("0", "").Should().Be("1");
        }

        [Fact]
        public void Adds_Additional_Digit_When_Needed()
        {
            var sut = new DecimalQuestionIntellisense(
                new DecimalQuestionParser(
                    new DecimalFormat(Scale.Zero),
                    RangeConstraint.Between(3m, 400m)),
                new DecimalFormat(Scale.Zero));

            using var _ = new AssertionScope();
            sut.NextValue("3", "3").Should().Be("30");
            sut.NextValue("30", "3").Should().Be("31");
            sut.NextValue("39", "3").Should().Be("300");
            sut.NextValue("300", "3").Should().Be("301");
            sut.NextValue("399", "3").Should().Be("3");

            sut.PreviousValue("3", "3").Should().Be("399");
            sut.PreviousValue("30", "3").Should().Be("3");
            sut.PreviousValue("39", "3").Should().Be("38");
            sut.PreviousValue("300", "3").Should().Be("39");
            sut.PreviousValue("399", "3").Should().Be("398");
        }
    }

    public class Scale2
    {
        private readonly DecimalQuestionIntellisense _sut = new(
            new DecimalQuestionParser(
                new DecimalFormat(Scale.Two),
                RangeConstraint.Between(900m, 5000m)),
            new DecimalFormat(Scale.Two));

        private static readonly IntellisenseUseCases UseCases = new()
        {
            FromUser = new List<IntellisenseUseCases.FromUserInput>
            {
                new() { Input = "",         Previous = "5,000.00",                        Next = "900.00",   UseCase = "No value" },
                new() { Input = "3",        Previous = "3,999.99",                        Next = "3,000.00", UseCase = "1 digit" },
                new() { Input = "30",       Previous = "3,099.99",                        Next = "3,000.00", UseCase = "2 digits" },
                new() { Input = "300",      Previous = "3,009.99",                        Next = "3,000.00", UseCase = "3 digits" },
                new() { Input = "3000",     Previous = "3,000.99", Complete = "3,000.00", Next = "3,000.01", UseCase = "4 digits" },
                new() { Input = "3000.1",   Previous = "3,000.19", Complete = "3,000.10", Next = "3,000.11", UseCase = "5 digits" },
                new() { Input = "3000.00",  Previous = "3,000.00",                        Next = "3,000.00", UseCase = "6 digits - valid value - unformatted" },
                new() { Input = "3,000.00", Previous = null,                              Next = null,       UseCase = "6 digits - valid value - formatted" },
                new() { Input = "8",        Previous = null,                              Next = null,       UseCase = "Incomplete - never in range" },
                new() { Input = "lala",     Previous = null,                              Next = null,       UseCase = "Invalid value" },
            },
            FromIntellisense = new List<IntellisenseUseCases.FromIntellisenseInput>
            {
                new() { Input = "3,000.00", Hint = "30", Previous = "3,099.99", Next = "3,000.01", UseCase = "Min value" },
                new() { Input = "3,050.50", Hint = "30", Previous = "3,050.49", Next = "3,050.51", UseCase = "Regular value" },
                new() { Input = "3,099.99", Hint = "30", Previous = "3,099.98", Next = "3,000.00", UseCase = "Max value" },
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

    public class Scale2EdgeCases
    {
        [Fact]
        public void Handle_0_In_Range()
        {
            var sut = new DecimalQuestionIntellisense(
                new DecimalQuestionParser(
                    new DecimalFormat(Scale.Two),
                    RangeConstraint.Between(-100m, 100m)),
                new DecimalFormat(Scale.Two));

            using var _ = new AssertionScope();
            sut.CompleteValue("0").Should().Be("0.00");
            sut.PreviousValue("0", "0").Should().Be("0.99");
            sut.NextValue("0", "0").Should().Be("0.01");
            sut.PreviousValue("0", "").Should().Be("-0.01");
            sut.NextValue("0", "").Should().Be("0.01");
        }
    }
}