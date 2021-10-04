using System.IO;

using FluentAssertions;

using Xunit;

namespace ConsoleAsksFor.Tests
{
    public class FileNameQuestionTests
    {
        private const string QuestionText = "Some Question";

        [Fact]
        public void Has_Correct_Guidance()
        {
            var question = new FileNameQuestion(QuestionText, FileSystemExistence.NewOrExisting, null);
            question.Text.Should().Be(QuestionText);
            question.GetHints().Should().BeEmpty();
        }

        [Fact]
        public void When_No_Default_Value_Has_No_PrefilledValue()
        {
            var question = new FileNameQuestion(QuestionText, FileSystemExistence.NewOrExisting, null);
            question.PrefilledValue.Should().BeEmpty();
        }

        [Fact]
        public void When_Default_Value_Has_Correct_PrefilledValue()
        {
            var question = new FileNameQuestion(QuestionText, FileSystemExistence.NewOrExisting, new FileInfo(@"C:\Test\Test.txt"));
            question.PrefilledValue.Should().Be(@"C:\Test\Test.txt");
        }

        internal static TheoryData<FileSystemExistence, string> CorrectParseUseCases() =>
            new()
            {
                { FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.NotExistingFile.Location },
                { FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location },
                { FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location.ToUpperInvariant() },
                { FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location },
                { FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.NotExistingFile.Location },
            };

        [Theory]
        [MemberData(nameof(CorrectParseUseCases))]
        internal void Parses_When_Correct_Value(FileSystemExistence fileSystemExistence, string fullName)
        {
            var question = new FileNameQuestion(QuestionText, fileSystemExistence, null);
            var isParsed = question.TryParse(fullName, out _, out var answer);
            isParsed.Should().BeTrue();
            answer!.FullName.Should().Be(fullName);
        }

        internal static TheoryData<FileSystemExistence, string, string> IncorrectParseUseCases() =>
            new()
            {
                { FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location, "File already exists." },
                { FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.ExistingFile1a.Location.ToUpperInvariant(), "File already exists." },
                { FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.NotExistingFile.Location, "File does not exists." },
                { FileSystemExistence.NewOrExisting, "", "Not a valid filename." },
                { FileSystemExistence.NewOrExisting, " ", "Not a valid filename." },
                { FileSystemExistence.NewOrExisting, @"Z:\Test\Test.txt", "Not a valid filename." },
                { FileSystemExistence.NewOrExisting, UnitTestFileSystem.NotExistingDirectoryWithInvalidChars.NotExistingFile.Location, "Not a valid filename." },
                { FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.NotExistingFileWithInvalidChars.Location, "Not a valid filename." },
                { FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.Location, "Expected file but found directory." },
                { FileSystemExistence.New, UnitTestFileSystem.ExistingDirectory1.Location, "Expected file but found directory." },
                { FileSystemExistence.Existing, UnitTestFileSystem.ExistingDirectory1.Location, "Expected file but found directory." },
                { FileSystemExistence.NewOrExisting, UnitTestFileSystem.ExistingDirectory1.NotExistingFileWithoutExtension.Location, "Extension missing." },
            };

        [Theory]
        [MemberData(nameof(IncorrectParseUseCases))]
        internal void Does_Not_Parse_When_Incorrect_Value(FileSystemExistence fileSystemExistence, string fullName, string error)
        {
            var question = new FileNameQuestion(QuestionText, fileSystemExistence, null);
            var isParsed = question.TryParse(fullName, out var errors, out _);
            isParsed.Should().BeFalse();
            errors.Should().ContainSingle().Which.Should().Be(error);
        }
    }
}