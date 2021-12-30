namespace ConsoleAsksFor;

internal interface IHistoryRepository
{
    Task<History> GetHistory();

    Task PersistHistory(History scopedHistory);
}