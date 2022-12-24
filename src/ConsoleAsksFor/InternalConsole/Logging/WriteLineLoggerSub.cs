namespace ConsoleAsksFor;

internal sealed class WriteLineLoggerSub : IWriteLineLogger
{
    public Task LogToFile(LineTypeId lineTypeId, string value)
        => Task.CompletedTask;
}
