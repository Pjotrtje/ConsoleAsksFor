using System.Collections.Generic;
using System.Linq;

namespace ConsoleAsksFor
{
    internal sealed class ScopedHistory : IScopedHistory
    {
        private readonly LinkedList<string> _items;
        private LinkedListNode<string>? _current;

        public IEnumerable<string> Items => _items;

        public ScopedHistory(IEnumerable<string> items)
            => _items = new LinkedList<string>(items);

        public string? MoveToNextAndGet()
        {
            if (!_items.Any())
            {
                return null;
            }

            if (_current is null)
            {
                return null;
            }

            _current = _current.Next ?? _current;
            return _current.Value;
        }

        public string? MoveToPreviousAndGet()
        {
            if (!_items.Any())
            {
                return null;
            }

            if (_current is null)
            {
                _current = _items.Last!;
                return _current.Value;
            }

            _current = _current.Previous ?? _current;
            return _current.Value;
        }
    }
}