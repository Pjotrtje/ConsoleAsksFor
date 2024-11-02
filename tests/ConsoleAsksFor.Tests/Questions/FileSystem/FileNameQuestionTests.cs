namespace ConsoleAsksFor.Tests;

public class FileNameQuestionTests
{
    private const string QuestionText = "Some Question";

    [Fact]
    public void Has_Correct_Guidance()
    {
        var question = new FileNameQuestion(QuestionText, FileSystemExistence.NewOrExisting, null, null);
        question.Text.Should().Be(QuestionText);
        question.GetHints().Should().BeEmpty();
    }

    [Fact]
    public void When_No_Default_Value_Has_No_PrefilledValue()
    {
        var question = new FileNameQuestion(QuestionText, FileSystemExistence.NewOrExisting, null, null);
        question.PrefilledValue.Should().BeEmpty();
    }

    [Fact]
    public void When_Default_Value_Has_Correct_PrefilledValue()
    {
        var question = new FileNameQuestion(QuestionText, FileSystemExistence.NewOrExisting, null, new FileInfo(@"C:\Test\Test.txt"));
        question.PrefilledValue.Should().Be(@"C:\Test\Test.txt");
    }

    private class CorrectParseUseCases : TheoryData<FileSystemExistence, string>
    {
        public CorrectParseUseCases()
        {
            Add(FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.NotExistingFile.Location);
            Add(FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location);
            Add(FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location.ToUpperInvariant());
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location);
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.NotExistingFile.Location);
        }
    }

    [Theory]
    [ClassData(typeof(CorrectParseUseCases))]
    internal void Parses_When_Correct_Value(FileSystemExistence fileSystemExistence, string fullName)
    {
        var question = new FileNameQuestion(QuestionText, fileSystemExistence, null, null);
        var isParsed = question.TryParse(fullName, out _, out var answer);
        isParsed.Should().BeTrue();
        answer!.FullName.Should().Be(fullName);
    }

    private class IncorrectParseUseCases : TheoryData<FileSystemExistence, string, string>
    {
        public IncorrectParseUseCases()
        {
            Add(FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location, "File already exists.");
            Add(FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location.ToUpperInvariant(), "File already exists.");
            Add(FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.NotExistingFile.Location, "File does not exists.");
            Add(FileSystemExistence.NewOrExisting, "", "Not a valid filename.");
            Add(FileSystemExistence.NewOrExisting, " ", "Not a valid filename.");
            Add(FileSystemExistence.NewOrExisting, @"Z:\Test\Test.txt", "Not a valid filename.");
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.NotExistingDirectoryWithInvalidChars.NotExistingFile.Location, "Not a valid filename.");
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.NotExistingFileWithInvalidChars.Location, "Not a valid filename.");
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.Location, "Expected file but found directory.");
            Add(FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.Location, "Expected file but found directory.");
            Add(FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.Location, "Expected file but found directory.");
            Add(FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.NotExistingFileWithoutExtension.Location, "Extension missing.");
        }
    }

    [Theory]
    [ClassData(typeof(IncorrectParseUseCases))]
    internal void Does_Not_Parse_When_Incorrect_Value(FileSystemExistence fileSystemExistence, string fullName, string error)
    {
        var question = new FileNameQuestion(QuestionText, fileSystemExistence, null, null);
        var isParsed = question.TryParse(fullName, out var errors, out _);
        isParsed.Should().BeFalse();
        errors.Should().ContainSingle().Which.Should().Be(error);
    }
}
