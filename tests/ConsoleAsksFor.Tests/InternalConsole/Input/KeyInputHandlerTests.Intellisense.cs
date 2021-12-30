namespace ConsoleAsksFor.Tests;

public partial class KeyInputHandlerTests
{
    public class Intellisense
    {
        private readonly Mock<IScopedHistory> _scopedHistory = new(MockBehavior.Strict);
        private readonly Mock<IIntellisense> _intellisense = new(MockBehavior.Strict);

        private readonly KeyInputHandler _sut = new();

        [Fact]
        public void CtrlSpace_When_ValueCanBeCompeted_Returns_Completed_As_Value()
        {
            var line = Line("1234").AtIndex(2);
            var expectedLine = line with
            {
                CursorIndex = 6,
                IntellisenseHint = "1234",
                Value = "123456",
            };

            _intellisense
                .Setup(i => i.CompleteValue(line.Value))
                .Returns("123456");

            var newLine = _sut.HandleKeyInput(line, CtrlSpace, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(expectedLine);
            VerifyAll();
        }

        [Fact]
        public void CtrlSpace_When_ValueCannotBeCompeted_Returns_Input()
        {
            var line = Line("1234").AtIndex(2);
            _intellisense
                .Setup(i => i.CompleteValue(line.Value))
                .Returns((string?)null);

            var newLine = _sut.HandleKeyInput(line, CtrlSpace, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAll();
        }

        [Fact]
        public void CtrlSpace_When_MustObfuscate_Returns_Input_And_DoesNotCall_Intellisense()
        {
            var line = Line("1234").AtIndex(2) with { MustObfuscate = true };

            var newLine = _sut.HandleKeyInput(line, CtrlSpace, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAll();
        }

        [Fact]
        public void Tab_When_HasNext_Returns_Next_As_Value()
        {
            var line = Line("1234").AtIndex(2);
            var expectedLine = line with
            {
                CursorIndex = 6,
                IntellisenseHint = "1234",
                Value = "123456",
            };

            _intellisense
                .Setup(i => i.NextValue(line.Value, line.IntellisenseHint))
                .Returns("123456");

            var newLine = _sut.HandleKeyInput(line, Tab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(expectedLine);
            VerifyAll();
        }

        [Fact]
        public void Tab_When_HasNoNext_Returns_Input()
        {
            var line = Line("1234").AtIndex(2);
            _intellisense
                .Setup(i => i.NextValue(line.Value, line.IntellisenseHint))
                .Returns((string?)null);

            var newLine = _sut.HandleKeyInput(line, Tab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAll();
        }

        [Fact]
        public void Tab_When_MustObfuscate_Returns_Input_And_DoesNotCall_Intellisense()
        {
            var line = Line("1234").AtIndex(2) with { MustObfuscate = true };

            var newLine = _sut.HandleKeyInput(line, Tab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAll();
        }

        [Fact]
        public void CtrlTab_When_HasNext_Returns_Next_As_Value()
        {
            var line = Line("1234").AtIndex(2);
            var expectedLine = line with
            {
                CursorIndex = 6,
                IntellisenseHint = "1234",
                Value = "123456",
            };

            _intellisense
                .Setup(i => i.NextValue(line.Value, ""))
                .Returns("123456");

            var newLine = _sut.HandleKeyInput(line, CtrlTab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(expectedLine);
            VerifyAll();
        }

        [Fact]
        public void CtrlTab_When_HasNoNext_Returns_Input()
        {
            var line = Line("1234").AtIndex(2);
            _intellisense
                .Setup(i => i.NextValue(line.Value, ""))
                .Returns((string?)null);

            var newLine = _sut.HandleKeyInput(line, CtrlTab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAll();
        }

        [Fact]
        public void CtrlTab_When_MustObfuscate_Returns_Input_And_DoesNotCall_Intellisense()
        {
            var line = Line("1234").AtIndex(2) with { MustObfuscate = true };

            var newLine = _sut.HandleKeyInput(line, CtrlTab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAll();
        }

        [Fact]
        public void ShiftTab_When_HasPrevious_Returns_Previous_As_Value()
        {
            var line = Line("1234").AtIndex(2);
            var expectedLine = line with
            {
                CursorIndex = 6,
                IntellisenseHint = "1234",
                Value = "123456",
            };

            _intellisense
                .Setup(i => i.PreviousValue(line.Value, line.IntellisenseHint))
                .Returns("123456");

            var newLine = _sut.HandleKeyInput(line, ShiftTab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(expectedLine);
            VerifyAll();
        }

        [Fact]
        public void ShiftTab_When_HasNoPrevious_Returns_Input()
        {
            var line = Line("1234").AtIndex(2);
            _intellisense
                .Setup(i => i.PreviousValue(line.Value, line.IntellisenseHint))
                .Returns((string?)null);

            var newLine = _sut.HandleKeyInput(line, ShiftTab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAll();
        }

        [Fact]
        public void ShiftTab_When_MustObfuscate_Returns_Input_And_DoesNotCall_Intellisense()
        {
            var line = Line("1234").AtIndex(2) with { MustObfuscate = true };

            var newLine = _sut.HandleKeyInput(line, ShiftTab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAll();
        }

        [Fact]
        public void CtrlShiftTab_When_HasPrevious_Returns_Previous_As_Value()
        {
            var line = Line("1234").AtIndex(2);
            var expectedLine = line with
            {
                CursorIndex = 6,
                IntellisenseHint = "1234",
                Value = "123456",
            };

            _intellisense
                .Setup(i => i.PreviousValue(line.Value, ""))
                .Returns("123456");

            var newLine = _sut.HandleKeyInput(line, CtrlShiftTab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(expectedLine);
            VerifyAll();
        }

        [Fact]
        public void CtrlShiftTab_When_HasNoPrevious_Returns_Input()
        {
            var line = Line("1234").AtIndex(2);
            _intellisense
                .Setup(i => i.PreviousValue(line.Value, ""))
                .Returns((string?)null);

            var newLine = _sut.HandleKeyInput(line, CtrlShiftTab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAll();
        }

        [Fact]
        public void CtrlShiftTab_When_MustObfuscate_Returns_Input_And_DoesNotCall_Intellisense()
        {
            var line = Line("1234").AtIndex(2) with { MustObfuscate = true };

            var newLine = _sut.HandleKeyInput(line, CtrlShiftTab, _scopedHistory.Object, _intellisense.Object);

            newLine.Should().BeEquivalentTo(line);
            VerifyAll();
        }

        private void VerifyAll()
        {
            _scopedHistory.VerifyAll();
            _intellisense.VerifyAll();
        }
    }
}