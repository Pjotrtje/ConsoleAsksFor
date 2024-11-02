namespace ConsoleAsksFor.Tests;

public class HistoryRepositoryTests
{
    private readonly Mock<IFileSystem> _fileSystem = new(MockBehavior.Strict);
    private readonly Mock<IConsoleLineWriter> _consoleLineWriter = new(MockBehavior.Strict);

    private static readonly IReadOnlyCollection<HistoryItem> SomeHistoryItems =
    [
        new HistoryItem("Q1", "QT1", "A1"),
        new HistoryItem("Q2", "QT2", "A2"),
    ];

    private const int HistoryMaxSize = 100;
    private const string FilePath = @".console\history.json";
    private const string DirectoryPath = @".console";

    private static readonly History SomeHistory = new(SomeHistoryItems, HistoryMaxSize);

    private static readonly string SomeHistoryItemsAsJson = @"
[
  {
    ""QuestionType"": ""Q1"",
    ""QuestionText"": ""QT1"",
    ""Answer"": ""A1""
  },
  {
    ""QuestionType"": ""Q2"",
    ""QuestionText"": ""QT2"",
    ""Answer"": ""A2""
  }
]".Trim();

    [Fact]
    public async Task Given_FilePath_Does_Not_Exist_When_GetHistory_Returns_Empty_But_Does_Not_Log()
    {
        var options = HistoryOptions.Default;
        _fileSystem
            .Setup(f => f.FileExists(FilePath))
            .Returns(false);

        var sut = CreateHistoryRepository(options);

        var result = await sut.GetHistory();

        result.Items.Should().BeEmpty();
        VerifyAllSetupsCalled();
    }

    [Fact]
    public async Task Given_Issues_With_IO_When_GetHistory_Returns_Empty_History_And_Logs_Error()
    {
        var options = HistoryOptions.Default;
        _fileSystem
            .Setup(f => f.FileExists(FilePath))
            .Returns(true);

        _fileSystem
            .Setup(f => f.FileReadAllTextAsync(FilePath))
            .Throws(new FileLoadException("Lala"));

        _consoleLineWriter
            .Setup(c => c.WriteErrorLine("GetHistory failed: Lala."));

        _consoleLineWriter
            .Setup(c => c.WriteWarningLine("History is not fetched. New history will not be persisted."));

        var sut = CreateHistoryRepository(options);

        var result = await sut.GetHistory();

        result.Items.Should().BeEmpty();
        VerifyAllSetupsCalled();
    }

    [Fact]
    public async Task When_GetHistory_Deserializes_Null_In_Property_Of_Items_File_Returns_Empty_History_And_Logs_Error()
    {
        var options = HistoryOptions.Default;
        _fileSystem
            .Setup(f => f.FileExists(FilePath))
            .Returns(true);

        _fileSystem
            .Setup(f => f.FileReadAllTextAsync(FilePath))
            .ReturnsAsync(
                @"[{
    ""QuestionType"": ""Q1"",
    ""QuestionText"": ""QT1"",
    ""Answer"": null
  }]");

        _consoleLineWriter
            .Setup(c => c.WriteErrorLine("GetHistory failed: Cannot map file content (json) to IReadOnlyCollection<HistoryItem>."));

        _consoleLineWriter
            .Setup(c => c.WriteWarningLine("History is not fetched. New history will not be persisted."));

        var sut = CreateHistoryRepository(options);

        var result = await sut.GetHistory();

        result.Items.Should().BeEmpty();
        VerifyAllSetupsCalled();
    }

    [Fact]
    public async Task When_GetHistory_OK_Returns_Deserialized_Items()
    {
        var options = HistoryOptions.Default;
        _fileSystem
            .Setup(f => f.FileExists(FilePath))
            .Returns(true);

        _fileSystem
            .Setup(f => f.FileReadAllTextAsync(FilePath))
            .ReturnsAsync(
                @"[{
    ""QuestionType"": ""Q1"",
    ""QuestionText"": ""QT1"",
    ""Answer"": ""A1""
  },
  {
    ""QuestionType"": ""Q2"",
    ""QuestionText"": ""QT2"",
    ""Answer"": ""A2""
  }]");

        var sut = CreateHistoryRepository(options);

        var result = await sut.GetHistory();

        result.Items.Should().BeEquivalentTo(SomeHistoryItems);

        VerifyAllSetupsCalled();
    }

    [Fact]
    public async Task Given_Issues_With_IO_When_PersistHistory_Logs_Error()
    {
        var options = HistoryOptions.Default;

        _fileSystem
            .Setup(f => f.CreateDirectory(DirectoryPath))
            .Throws(new FileLoadException("Lala"));

        _consoleLineWriter
            .Setup(c => c.WriteErrorLine("PersistHistory failed: Lala."));

        _consoleLineWriter
            .Setup(c => c.WriteWarningLine("History is not persisted. No more attempts are made during execution of this consoleapp."));

        var sut = CreateHistoryRepository(options);

        await sut.PersistHistory(SomeHistory);

        VerifyAllSetupsCalled();
    }

    [Fact]
    public async Task When_PersistHistory_OK_Persists_Items()
    {
        var options = HistoryOptions.Default;

        _fileSystem
            .Setup(f => f.CreateDirectory(DirectoryPath));

        _fileSystem
            .Setup(f => f.FileWriteAllTextAsync(FilePath, SomeHistoryItemsAsJson))
            .Returns(Task.CompletedTask);

        var sut = CreateHistoryRepository(options);

        await sut.PersistHistory(SomeHistory);

        VerifyAllSetupsCalled();
    }

    [Fact]
    public async Task Given_Error_During_GetHistory_When_PersistHistory_The_Persist_Is_Skipped()
    {
        var options = HistoryOptions.Default;
        _fileSystem
            .Setup(f => f.FileExists(FilePath))
            .Returns(true);

        _fileSystem
            .Setup(f => f.FileReadAllTextAsync(FilePath))
            .Throws(new FileLoadException("Lala"));

        _consoleLineWriter
            .Setup(c => c.WriteErrorLine("GetHistory failed: Lala."));

        _consoleLineWriter
            .Setup(c => c.WriteWarningLine("History is not fetched. New history will not be persisted."));

        var sut = CreateHistoryRepository(options);

        var result = await sut.GetHistory();
        await sut.PersistHistory(result);

        VerifyAllSetupsCalled();
    }

    [Fact]
    public async Task Given_No_Error_During_GetHistory_When_PersistHistory_The_Persist_Is_Not_Skipped()
    {
        var options = HistoryOptions.Default;
        _fileSystem
            .Setup(f => f.FileExists(FilePath))
            .Returns(true);

        _fileSystem
            .Setup(f => f.FileReadAllTextAsync(FilePath))
            .ReturnsAsync(SomeHistoryItemsAsJson);

        _fileSystem
            .Setup(f => f.CreateDirectory(DirectoryPath));

        _fileSystem
            .Setup(f => f.FileWriteAllTextAsync(FilePath, SomeHistoryItemsAsJson))
            .Returns(Task.CompletedTask);

        var sut = CreateHistoryRepository(options);

        var result = await sut.GetHistory();
        await sut.PersistHistory(result);

        VerifyAllSetupsCalled();
    }

    private HistoryRepository CreateHistoryRepository(HistoryOptions options)
        => new(
            _fileSystem.Object,
            _consoleLineWriter.Object,
            FilePath,
            options.MaxSize);

    private void VerifyAllSetupsCalled()
    {
        _consoleLineWriter.VerifyAll();
        _fileSystem.VerifyAll();
    }
}