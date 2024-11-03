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

    private class CorrectParseUseCases : TheoryData<FileSystemExistence, string>
    {
        public CorrectParseUseCases()
        {
            Add(FileSystemExistence.New, UnitTestFileSystem.NotExistingDirectory.Location);
            Add(FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.Location);
            Add(FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.Location.ToUpperInvariant());
            Add(FileSystemExistence.Existing, UnitTestFileSystem.Drive.Location);
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.Location);
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.Drive.Location);
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.NotExistingDirectory.Location);
        }
    }

    [Theory]
    [ClassData(typeof(CorrectParseUseCases))]
    internal void Parses_When_Correct_Value(FileSystemExistence fileSystemExistence, string fullName)
    {
        var question = new DirectoryQuestion(QuestionText, fileSystemExistence, null);
        var isParsed = question.TryParse(fullName, out _, out var answer);
        isParsed.Should().BeTrue();
        answer!.FullName.Should().Be(fullName);
    }

    private class IncorrectParseUseCases : TheoryData<FileSystemExistence, string, string>
    {
        public IncorrectParseUseCases()
        {
            Add(FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.Location, "Directory already exists.");
            Add(FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.Location.ToUpperInvariant(), "Directory already exists.");
            Add(FileSystemExistence.Existing, UnitTestFileSystem.NotExistingDirectory.Location, "Directory does not exists.");
            Add(FileSystemExistence.NewOrExisting, "", "Not a valid directory.");
            Add(FileSystemExistence.NewOrExisting, " ", "Not a valid directory.");
            Add(FileSystemExistence.NewOrExisting, @"Z:\Test", "Not a valid directory.");
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.NotExistingDirectoryWithInvalidChars.Location, "Not a valid directory.");
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location, "Expected directory but found file.");
            Add(FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location, "Expected directory but found file.");
            Add(FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location, "Expected directory but found file.");
        }
    }

    [Theory]
    [ClassData(typeof(IncorrectParseUseCases))]
    internal void Does_Not_Parse_When_Incorrect_Value(FileSystemExistence fileSystemExistence, string fullName, string error)
    {
        var question = new DirectoryQuestion(QuestionText, fileSystemExistence, null);
        var isParsed = question.TryParse(fullName, out var errors, out _);
        isParsed.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be(error);
    }
}
