namespace ConsoleAsksFor.Tests;

public class FileSystemQuestionIntellisenseTests
{
    private readonly FileSystemQuestionIntellisense _sut = new(true, null);

    private static readonly IntellisenseUseCases UseCases = new()
    {
        FromUser = new List<IntellisenseUseCases.FromUserInput>
        {
            new()
            {
                Input = "",
                Previous = null,
                Next = null,
                UseCase = "No value",
            },
            new()
            {
                Input = UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location,
                Previous = null,
                Next = null,
                UseCase = "ExistingFile",
            },
            new()
            {
                Input = UnitTestFileSystem.Location,
                Previous = null,
                Next = null,
                UseCase = "ExistingDirectory",
            },
            new()
            {
                Input = UnitTestFileSystem.Location + "\\",
                Previous = UnitTestFileSystem.ExistingDirectory2.Location,
                Next = UnitTestFileSystem.ExistingDirectory1.Location,
                UseCase = "ExistingDirectory with slashes" ,
            },
            new()
            {
                // Questionable test...
                Input = UnitTestFileSystem.Drive.Location,
                Previous = Directory.GetFileSystemEntries(UnitTestFileSystem.Drive.Location).Last(),
                Next = Directory.GetFileSystemEntries(UnitTestFileSystem.Drive.Location).First(),
                UseCase = "Existing Drive",
            },
            new()
            {
                Input = UnitTestFileSystem.ExistingDirectory1.Location.SkipLast(3).JoinToString(),
                Previous = UnitTestFileSystem.ExistingDirectory2.Location,
                Next = UnitTestFileSystem.ExistingDirectory1.Location,
                UseCase = "Almost Existing directory",
            },
            new()
            {
                Input = UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location.SkipLast(6).JoinToString(),
                Previous = UnitTestFileSystem.ExistingDirectory1.ExistingFile1b.Location,
                Next = UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location,
                UseCase = "Almost Existing file",
            },
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
