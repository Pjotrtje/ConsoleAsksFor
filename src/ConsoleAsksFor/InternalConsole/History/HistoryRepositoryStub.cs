﻿namespace ConsoleAsksFor;

internal sealed class HistoryRepositoryStub : IHistoryRepository
{
    private readonly int _maxSize;

    public HistoryRepositoryStub(int maxSize)
    {
        _maxSize = maxSize;
    }

    public Task<History> GetHistory()
    {
        var history = new History([], _maxSize);
        return Task.FromResult(history);
    }

    public Task PersistHistory(History scopedHistory)
        => Task.CompletedTask;
}