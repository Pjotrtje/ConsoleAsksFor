namespace ConsoleAsksFor.Tests;

public partial class KeyInputHandlerTests
{
    public class History
    {
        private readonly Mock<IScopedHistory> _scopedHistory = new(MockBehavior.Strict);
        private readonly Mock<IIntellisense> _intellisense = new(MockBehavior.Strict);

        private readonly KeyInputHandler _sut = new();

        [Fact]
        public void UpArrow_When_History_Returns_History_As_Value()
        {
            var line = Line("1234").AtIndex(2);
            var expectedLine = line with
            {
                CursorIndex = 3,
                IntellisenseHint = "345",
                Value = "345",
            };

            _scopedHistory
                .Setup(h => h.MoveToPreviousAndGet())
                .Returns(expectedLine.Value);

            var newLine = _sut.HandleKeyInput(line, UpArrow, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(expectedLine);
            VerifyAllSetupsCalled();
        }

        [Fact]
        public void UpArrow_When_No_History_Returns_Input()
        {
            var line = Line("1234").AtIndex(2);
            _scopedHistory
                .Setup(h => h.MoveToPreviousAndGet()).Returns((string?)null);

            var newLine = _sut.HandleKeyInput(line, UpArrow, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAllSetupsCalled();
        }

        [Fact]
        public void UpArrow_When_MustObfuscate_Returns_Input_And_DoesNotCall_History()
        {
            var line = Line("1234").AtIndex(2) with { MustObfuscate = true };

            var newLine = _sut.HandleKeyInput(line, UpArrow, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAllSetupsCalled();
        }

        [Fact]
        public void DownArrow_When_History_Returns_History_As_Value()
        {
            var line = Line("1234").AtIndex(2);
            var expectedLine = line with
            {
                CursorIndex = 3,
                IntellisenseHint = "345",
                Value = "345",
            };

            _scopedHistory
                .Setup(h => h.MoveToNextAndGet())
                .Returns(expectedLine.Value);

            var newLine = _sut.HandleKeyInput(line, DownArrow, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(expectedLine);
            VerifyAllSetupsCalled();
        }

        [Fact]
        public void DownArrow_When_No_History_Returns_Input()
        {
            var line = Line("1234").AtIndex(2);
            _scopedHistory
                .Setup(h => h.MoveToNextAndGet())
                .Returns((string?)null);

            var newLine = _sut.HandleKeyInput(line, DownArrow, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAllSetupsCalled();
        }

        [Fact]
        public void DownArrow_When_MustObfuscate_Returns_Input_And_DoesNotCall_History()
        {
            var line = Line("1234").AtIndex(2) with { MustObfuscate = true };
            var newLine = _sut.HandleKeyInput(line, DownArrow, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAllSetupsCalled();
        }

        private void VerifyAllSetupsCalled()
        {
            _scopedHistory.VerifyAll();
            _intellisense.VerifyAll();
        }
    }
}