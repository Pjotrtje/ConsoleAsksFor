namespace ConsoleAsksFor.Tests;

public partial class KeyInputHandlerTests
{
    private readonly Mock<IScopedHistory> _scopedHistory = new(MockBehavior.Strict);
    private readonly Mock<IIntellisense> _intellisense = new(MockBehavior.Strict);

    private readonly KeyInputHandler _sut = new();

    private static InProgressLine Line(string value)
        => new(value, false)
        {
            IntellisenseHint = value,
        };

    [Theory]
    [ClassData(typeof(MoveCursorLeftUseCases))]
    [ClassData(typeof(MoveCursorRightUseCases))]
    [ClassData(typeof(DeleteUseCases))]
    [ClassData(typeof(BackspaceUseCases))]
    [ClassData(typeof(OtherUseCases))]
    internal void HandleKeyInput(InProgressLine line, KeyInput keyInput, InProgressLine expectedNewLine, string useCase)
    {
        var newLine = _sut.HandleKeyInput(line, keyInput, _scopedHistory.Object, _intellisense.Object);
        newLine.Should().BeEquivalentTo(expectedNewLine, useCase);
        VerifyAllSetupsCalled();
    }

    private class OtherUseCases : TheoryData<InProgressLine, KeyInput, InProgressLine, string>
    {
        public OtherUseCases()
        {
            Add(Line("0123").AtIndex(0), FromChar('a'), Line("a0123").AtIndex(1), "Regular char; at end");
            Add(Line("0123").AtIndex(4), FromChar('a'), Line("0123a").AtIndex(5), "Regular char; at end");
            Add(Line("0123").AtIndex(2), FromChar('a'), Line("01a23").AtIndex(3), "Regular char; in middle");

            Add(Line("0123").AtIndex(0), Escape, Line("").AtIndex(0), "Escape; At start");
            Add(Line("0123").AtIndex(2), Escape, Line("").AtIndex(0), "Escape; In middle");
            Add(Line("0123").AtIndex(4), Escape, Line("").AtIndex(0), "Escape; At end");

            Add(Line("0123").AtIndex(4), F3, Line("0123").AtIndex(4), "A to ignore key");
        }
    }

    private class BackspaceUseCases : TheoryData<InProgressLine, KeyInput, InProgressLine, string>
    {
        public BackspaceUseCases()
        {
            Add(Line("0123").AtIndex(0), Backspace, Line("0123").AtIndex(0), "Backspace: At start");
            Add(Line("0123").AtIndex(2), Backspace, Line("023").AtIndex(1), "Backspace: In middle");
            Add(Line("0123").AtIndex(4), Backspace, Line("012").AtIndex(3), "Backspace: At end");
            Add(Line(" 1234 6789 ").AtIndex(0), CtrlBackspace, Line(" 1234 6789 ").AtIndex(0), "CtrlBackspace: At begin");
            Add(Line(" 1234 6789 ").AtIndex(1), CtrlBackspace, Line("1234 6789 ").AtIndex(0), "CtrlBackspace: At begin first word");
            Add(Line(" 1234 6789 ").AtIndex(3), CtrlBackspace, Line(" 34 6789 ").AtIndex(1), "CtrlBackspace: In first word");
            Add(Line(" 1234 6789 ").AtIndex(5), CtrlBackspace, Line("  6789 ").AtIndex(1), "CtrlBackspace: At end of first word");
            Add(Line(" 1234 6789 ").AtIndex(6), CtrlBackspace, Line(" 6789 ").AtIndex(1), "CtrlBackspace: At start of second word");
            Add(Line(" 1234 6789 ").AtIndex(8), CtrlBackspace, Line(" 1234 89 ").AtIndex(6), "CtrlBackspace: In second word");
            Add(Line(" 1234 6789 ").AtIndex(11), CtrlBackspace, Line(" 1234 ").AtIndex(6), "CtrlBackspace: At end");
        }
    }

    private class DeleteUseCases : TheoryData<InProgressLine, KeyInput, InProgressLine, string>
    {
        public DeleteUseCases()
        {
            Add(Line("0123").AtIndex(0), Delete, Line("123").AtIndex(0), "Delete: At start");
            Add(Line("0123").AtIndex(2), Delete, Line("013").AtIndex(2), "Delete: In middle");
            Add(Line("0123").AtIndex(4), Delete, Line("0123").AtIndex(4), "Delete: At end");
            Add(Line(" 1234 6789 ").AtIndex(11), CtrlDelete, Line(" 1234 6789 ").AtIndex(11), "CtrlDelete: At end");
            Add(Line(" 1234 6789 ").AtIndex(10), CtrlDelete, Line(" 1234 6789").AtIndex(10), "CtrlDelete: At end last word");
            Add(Line(" 1234 6789 ").AtIndex(8), CtrlDelete, Line(" 1234 67").AtIndex(8), "CtrlDelete: In last word");
            Add(Line(" 1234 6789 ").AtIndex(6), CtrlDelete, Line(" 1234 ").AtIndex(6), "CtrlDelete: At begin of last word");
            Add(Line(" 1234 6789 ").AtIndex(5), CtrlDelete, Line(" 12346789 ").AtIndex(5), "CtrlDelete: At end of second from last word");
            Add(Line(" 1234 6789 ").AtIndex(1), CtrlDelete, Line(" 6789 ").AtIndex(1), "CtrlDelete: In second from last word");
            Add(Line(" 1234 6789 ").AtIndex(0), CtrlDelete, Line("1234 6789 ").AtIndex(0), "CtrlDelete: At begin");
        }
    }

    private class MoveCursorLeftUseCases : TheoryData<InProgressLine, KeyInput, InProgressLine, string>
    {
        public MoveCursorLeftUseCases()
        {
            Add(Line("0123").AtIndex(0), Home, Line("0123").AtIndex(0), "Home: At begin");
            Add(Line("0123").AtIndex(2), Home, Line("0123").AtIndex(0), "Home: In middle");
            Add(Line("0123").AtIndex(4), Home, Line("0123").AtIndex(0), "Home: At end");
            Add(Line("0123").AtIndex(0), LeftArrow, Line("0123").AtIndex(0), "LeftArrow: At begin");
            Add(Line("0123").AtIndex(2), LeftArrow, Line("0123").AtIndex(1), "LeftArrow: In middle");
            Add(Line("0123").AtIndex(4), LeftArrow, Line("0123").AtIndex(3), "LeftArrow: At end");
            Add(Line(" 1234 6789 ").AtIndex(0), CtrlLeftArrow, Line(" 1234 6789 ").AtIndex(0), "CtrlLeftArrow: At begin");
            Add(Line(" 1234 6789 ").AtIndex(1), CtrlLeftArrow, Line(" 1234 6789 ").AtIndex(0), "CtrlLeftArrow: At begin first word");
            Add(Line(" 1234 6789 ").AtIndex(3), CtrlLeftArrow, Line(" 1234 6789 ").AtIndex(1), "CtrlLeftArrow: In first word");
            Add(Line(" 1234 6789 ").AtIndex(5), CtrlLeftArrow, Line(" 1234 6789 ").AtIndex(1), "CtrlLeftArrow: At end of first word");
            Add(Line(" 1234 6789 ").AtIndex(6), CtrlLeftArrow, Line(" 1234 6789 ").AtIndex(1), "CtrlLeftArrow: At start of second word");
            Add(Line(" 1234 6789 ").AtIndex(8), CtrlLeftArrow, Line(" 1234 6789 ").AtIndex(6), "CtrlLeftArrow: In second word");
            Add(Line(" 1234 6789 ").AtIndex(11), CtrlLeftArrow, Line(" 1234 6789 ").AtIndex(6), "CtrlLeftArrow: At end");
        }
    }

    private class MoveCursorRightUseCases : TheoryData<InProgressLine, KeyInput, InProgressLine, string>
    {
        public MoveCursorRightUseCases()
        {
            Add(Line("0123").AtIndex(0), End, Line("0123").AtIndex(4), "End: At begin");
            Add(Line("0123").AtIndex(2), End, Line("0123").AtIndex(4), "End: In middle");
            Add(Line("0123").AtIndex(4), End, Line("0123").AtIndex(4), "End: At end");
            Add(Line("0123").AtIndex(0), RightArrow, Line("0123").AtIndex(1), "RightArrow: At begin");
            Add(Line("0123").AtIndex(2), RightArrow, Line("0123").AtIndex(3), "RightArrow: In middle");
            Add(Line("0123").AtIndex(4), RightArrow, Line("0123").AtIndex(4), "RightArrow: At end");
            Add(Line(" 1234 6789 ").AtIndex(11), CtrlRightArrow, Line(" 1234 6789 ").AtIndex(11), "CtrlRightArrow: At end");
            Add(Line(" 1234 6789 ").AtIndex(10), CtrlRightArrow, Line(" 1234 6789 ").AtIndex(11), "CtrlRightArrow: At end last word");
            Add(Line(" 1234 6789 ").AtIndex(8), CtrlRightArrow, Line(" 1234 6789 ").AtIndex(11), "CtrlRightArrow: In last word");
            Add(Line(" 1234 6789 ").AtIndex(6), CtrlRightArrow, Line(" 1234 6789 ").AtIndex(11), "CtrlRightArrow: At begin of last word");
            Add(Line(" 1234 6789 ").AtIndex(5), CtrlRightArrow, Line(" 1234 6789 ").AtIndex(6), "CtrlRightArrow: At end of second from last word");
            Add(Line(" 1234 6789 ").AtIndex(1), CtrlRightArrow, Line(" 1234 6789 ").AtIndex(6), "CtrlRightArrow: In second from last word");
            Add(Line(" 1234 6789 ").AtIndex(0), CtrlRightArrow, Line(" 1234 6789 ").AtIndex(1), "CtrlRightArrow: At begin");
        }
    }

    private void VerifyAllSetupsCalled()
    {
        _scopedHistory.VerifyAll();
        _intellisense.VerifyAll();
    }
}
