namespace ConsoleAsksFor;

internal interface IKeyInputHandler
{
    InProgressLine HandleKeyInput(InProgressLine line, KeyInput keyInput, IScopedHistory scopedHistory, IIntellisense intellisense);
}