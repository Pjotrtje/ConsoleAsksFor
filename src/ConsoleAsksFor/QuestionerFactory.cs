using System;
using System.Threading.Tasks;

using ConsoleAsksFor.Sdk;

namespace ConsoleAsksFor
{
    internal sealed class QuestionerFactory : IQuestionerFactory
    {
        private readonly IConsoleLineWriter _consoleLineWriter;
        private readonly IConsoleInputGetter _consoleInputGetter;
        private readonly IKeyInputHandler _keyInputHandler;
        private readonly IHistoryRepository _historyRepository;
        private readonly Lazy<Task<History>> _lazyHistory;

        public QuestionerFactory(
            IConsoleLineWriter consoleLineWriter,
            IConsoleInputGetter consoleInputGetter,
            IKeyInputHandler keyInputHandler,
            IHistoryRepository historyRepository)
        {
            _consoleLineWriter = consoleLineWriter;
            _consoleInputGetter = consoleInputGetter;
            _keyInputHandler = keyInputHandler;
            _historyRepository = historyRepository;
            _lazyHistory = new Lazy<Task<History>>(_historyRepository.GetHistory);
        }

        public async Task<Questioner<TAnswer>> Create<TAnswer>(
            IQuestion<TAnswer> question)
            where TAnswer : notnull
        {
            return new(
                _consoleLineWriter,
                _consoleInputGetter,
                _keyInputHandler,
                _historyRepository,
                await _lazyHistory.Value,
                question);
        }
    }
}