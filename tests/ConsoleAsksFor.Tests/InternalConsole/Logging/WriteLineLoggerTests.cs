using System;
using System.IO;
using System.Threading.Tasks;

using FluentAssertions.Extensions;

using Moq;

using Xunit;

namespace ConsoleAsksFor.Tests
{
    public class WriteLineLoggerTests
    {
        private readonly Mock<IFileSystem> _fileSystem = new(MockBehavior.Strict);
        private readonly Mock<ISuspendableOutWriter> _suspendableOutWriter = new(MockBehavior.Strict);
        private readonly Mock<IDateTimeProvider> _dateTimeProvider = new(MockBehavior.Strict);

        private static readonly LineTypes LineTypes = new(ConsoleColors.Default);

        private const string SomeLogValue = nameof(SomeLogValue);
        private const string DirectoryPath = @".console\logs";

        private static readonly DateTime StartTime = 2.January(2020).At(19, 10, 11);

        [Fact]
        public async Task Given_LineType_Not_For_Log_When_LogToFile_Does_Not_Log()
        {
            var options = LoggingOptions.Default with
            {
                ToLogLineTypes = new[] { LineTypeId.Question },
            };

            var sut = CreateWriteLineLogger(options);
            await sut.LogToFile(LineTypeId.Answer, SomeLogValue);

            VerifyAllSetupsCalled();
        }

        [Fact]
        public async Task Given_Issues_With_IO_When_LogToFile_Does_Not_Log_To_File_And_Logs_Error_To_Console_And_Subsequent_Calls_Do_Not_Log()
        {
            var options = LoggingOptions.Default with
            {
                ToLogLineTypes = new[] { LineTypeId.Question },
            };

            _dateTimeProvider
                .Setup(d => d.Now)
                .Returns(DateTime.Now);

            _fileSystem
                .Setup(f => f.CreateDirectory(DirectoryPath))
                .Throws(new FileLoadException("Lala"));

            _suspendableOutWriter
                .Setup(s => s.WriteLine(It.Is<string>(l => l.Contains("LogToFile failed: Lala."))));

            _suspendableOutWriter
                .Setup(s => s.WriteLine(It.Is<string>(l => l.Contains("Console output is not logged. New console output will not be logged."))));

            var sut = CreateWriteLineLogger(options);
            await sut.LogToFile(LineTypeId.Question, SomeLogValue);
            await sut.LogToFile(LineTypeId.Question, SomeLogValue);

            VerifyAllSetupsCalled();
            _dateTimeProvider.Verify(d => d.Now, Times.Once); // if tried to log twice, this should be called twice
        }

        [Fact]
        public async Task Given_LogToFile_Ok_Logs_To_File_And_Adds_Prefix_For_Every_Line()
        {
            const string someOtherLogValue = nameof(someOtherLogValue);
            var options = LoggingOptions.Default with
            {
                ToLogLineTypes = new[] { LineTypeId.Question },
            };

            var now = 3.January(2021).At(20, 11, 31);
            const string nowAsString = "2021-01-03 20:11:31";
            var logFileNamePath = $@"{DirectoryPath}\2020-01-02_19.10.11.log";

            var toLog = SomeLogValue + Environment.NewLine + someOtherLogValue;
            var expectedLoggedItems = new[]
            {
                $"{nowAsString} | Question | {SomeLogValue}",
                $"{nowAsString} | Question | {someOtherLogValue}",
            };

            _dateTimeProvider
                .Setup(d => d.Now)
                .Returns(now);

            _fileSystem
                .Setup(f => f.CreateDirectory(DirectoryPath));

            _fileSystem
                .Setup(f => f.FileAppendAllLinesAsync(logFileNamePath, expectedLoggedItems))
                .Returns(Task.CompletedTask);

            var sut = CreateWriteLineLogger(options);
            await sut.LogToFile(LineTypeId.Question, toLog);

            VerifyAllSetupsCalled();
        }

        private WriteLineLogger CreateWriteLineLogger(LoggingOptions options)
            => new(
                _fileSystem.Object,
                _suspendableOutWriter.Object,
                _dateTimeProvider.Object,
                LineTypes,
                options,
                StartTime);

        private void VerifyAllSetupsCalled()
        {
            _suspendableOutWriter.VerifyAll();
            _dateTimeProvider.VerifyAll();
            _fileSystem.VerifyAll();
        }
    }
}