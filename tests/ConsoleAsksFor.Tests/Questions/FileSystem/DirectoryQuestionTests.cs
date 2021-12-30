namespace ConsoleAsksFor.Tests;

public class DirectoryQuestionTests
{
    private const string QuestionText = "Some Question";

    [Fact]
    public void Has_Correct_Guidance()
    {
        var question = new DirectoryQuestion(QuestionText, FileSystemExistence.NewOrExisting, null);
        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEmpty();
    }

    [Fact]
    public void When_No_Default_Value_Has_No_PrefilledValue()
    {
        var question = new DirectoryQuestion(QuestionText, FileSystemExistence.NewOrExisting, null);
        question.PrefilledValue.Should().BeEmpty();
    }

    [Fact]
    public void When_Default_Value_Has_Correct_PrefilledValue()
    {
        var question = new DirectoryQuestion(QuestionText, FileSystemExistence.NewOrExisting, new DirectoryInfo(@"C:\Test\"));
        question.PrefilledValue.Should().Be(@"C:\Test\");
    }

    internal static TheoryData<FileSystemExistence, string> CorrectParseUseCases() =>
        new()
        {
            { FileSystemExistence.New, UnitTestFileSystem.NotExistingDirectory.Location },
            { FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.Location },
            { FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.Location.ToUpperInvariant() },
            { FileSystemExistence.Existing, UnitTestFileSystem.Drive.Location },
            { FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.Location },
            { FileSystemExistence.NewOrExisting, UnitTestFileSystem.Drive.Location },
            { FileSystemExistence.NewOrExisting, UnitTestFileSystem.NotExistingDirectory.Location },
        };

    [Theory]
    [MemberData(nameof(CorrectParseUseCases))]
    internal void Parses_When_Correct_Value(FileSystemExistence fileSystemExistence, string fullName)
    {
        var question = new DirectoryQuestion(QuestionText, fileSystemExistence, null);
        var isParsed = question.TryParse(fullName, out _, out var answer);
        isParsed.Should().BeTrue();
        answer!.FullName.Should().Be(fullName);
    }

    internal static TheoryData<FileSystemExistence, string, string> IncorrectParseUseCases() =>
        new()
        {
            { FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.Location, "Directory already exists." },
            { FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.Location.ToUpperInvariant(), "Directory already exists." },
            { FileSystemExistence.Existing, UnitTestFileSystem.NotExistingDirectory.Location, "Directory does not exists." },
            { FileSystemExistence.NewOrExisting, "", "Not a valid directory." },
            { FileSystemExistence.NewOrExisting, " ", "Not a valid directory." },
            { FileSystemExistence.NewOrExisting, @"Z:\Test", "Not a valid directory." },
            { FileSystemExistence.NewOrExisting, UnitTestFileSystem.NotExistingDirectoryWithInvalidChars.Location, "Not a valid directory." },
            { FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location, "Expected directory but found file." },
            { FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location, "Expected directory but found file." },
            { FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location, "Expected directory but found file." },
        };

    [Theory]
    [MemberData(nameof(IncorrectParseUseCases))]
    internal void Does_Not_Parse_When_Incorrect_Value(FileSystemExistence fileSystemExistence, string fullName, string error)
    {
        var question = new DirectoryQuestion(QuestionText, fileSystemExistence, null);
        var isParsed = question.TryParse(fullName, out var errors, out _);
        isParsed.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be(error);
    }
}