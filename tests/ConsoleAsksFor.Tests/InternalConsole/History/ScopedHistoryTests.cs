using System;

using FluentAssertions;

using Xunit;

namespace ConsoleAsksFor.Tests;

public class ScopedHistoryTests
{
    public class EmptyScopedHistory
    {
        private readonly ScopedHistory _sut = new(Array.Empty<string>());

        [Fact]
        public void Items_Should_Be_Empty()
        {
            _sut.Items.Should().BeEmpty();
        }

        [Fact]
        public void MoveToNextAndGet_Returns_Null()
        {
            _sut.MoveToNextAndGet().Should().BeNull();
        }

        [Fact]
        public void MoveToPreviousAndGet_Returns_Null()
        {
            _sut.MoveToPreviousAndGet().Should().BeNull();
        }
    }

    public class FilledScopedHistory
    {
        private const string Item1 = nameof(Item1);
        private const string Item2 = nameof(Item2);

        private readonly ScopedHistory _sut = new(new[] { Item1, Item2 });

        [Fact]
        public void Items_Should_Be_Contains_Input_From_Constructor()
        {
            _sut.Items.Should().BeEquivalentTo(Item1, Item2);
        }

        [Fact]
        public void MoveToNextAndGet_Returns_Null()
        {
            _sut.MoveToNextAndGet().Should().BeNull();
        }

        [Fact]
        public void MoveToPreviousAndGet_Returns_Last()
        {
            _sut.MoveToPreviousAndGet().Should().Be(Item2);
        }

        [Fact]
        public void Twice_MoveToPreviousAndGet_Returns_BeforeLast()
        {
            _sut.MoveToPreviousAndGet();
            _sut.MoveToPreviousAndGet().Should().Be(Item1);
        }

        [Fact]
        public void MoveToPreviousAndGet_MoveToNextAndGet_Returns_Last()
        {
            _sut.MoveToPreviousAndGet();
            _sut.MoveToNextAndGet().Should().Be(Item2);
        }

        [Fact]
        public void After_A_Lot_MoveToPreviousAndGet_Returns_First()
        {
            _sut.MoveToPreviousAndGet();
            _sut.MoveToPreviousAndGet();
            _sut.MoveToPreviousAndGet();
            _sut.MoveToPreviousAndGet().Should().Be(Item1);
        }

        [Fact]
        public void After_MoveToPreviousAndGet_And_A_Lot_Of_MoveToNextAndGet_Returns_Last()
        {
            _sut.MoveToPreviousAndGet();
            _sut.MoveToNextAndGet();
            _sut.MoveToNextAndGet();
            _sut.MoveToNextAndGet();
            _sut.MoveToNextAndGet();
            _sut.MoveToNextAndGet().Should().Be(Item2);
        }
    }
}