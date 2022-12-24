namespace ConsoleAsksFor;

internal interface IWriteLineLogger
{
    Task LogToFile(LineTypeId lineTypeId, string value);
}
