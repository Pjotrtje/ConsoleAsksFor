namespace ConsoleAsksFor.Tests;

public class HistoryTests
{
    private readonly History _sut = new(Array.Empty<HistoryItem>(), int.MaxValue);

    private const string IntQuestion = nameof(IntQuestion);
    private const string DecimalQuestion = nameof(DecimalQuestion);
    private const string Who = nameof(Who);
    private const string Why = nameof(Why);

    [Fact]
    public void Add_Adds_Items()
    {
        var item1 = new HistoryItem(IntQuestion, Who, "1");
        var item2 = new HistoryItem(IntQuestion, Who, "2");

        _sut.Add(item1);
        _sut.Add(item2);

        _sut.Items.Should().BeEquivalentTo(new[]
        {
            item1,
            item2,
        });
    }

    [Fact]
    public void Add_Adds_Moves_Already_Added_ToLast_Item()
    {
        var item1 = new HistoryItem(IntQuestion, Who, "1");
        var item2 = new HistoryItem(IntQuestion, Who, "2");

        _sut.Add(item1);
        _sut.Add(item2);
        _sut.Add(item1);

        _sut.Items.Should().BeEquivalentTo(new[]
        {
            item2,
            item1,
        });
    }

    [Fact]
    public void GetScopedHistory_ByQuestion_Returns_Only_History_For_Question()
    {
        _sut.Add(new HistoryItem(IntQuestion, Who, "1"));
        _sut.Add(new HistoryItem(IntQuestion, Why, "2"));
        _sut.Add(new HistoryItem(IntQuestion, Who, "3"));
        _sut.Add(new HistoryItem(IntQuestion, Why, "4"));
        _sut.Add(new HistoryItem(DecimalQuestion, Who, "5"));

        var scopedHistory = _sut.GetScopedHistory(HistoryType.ByQuestionTextAndType, IntQuestion, Who);
        scopedHistory.Items.Should().BeEquivalentTo("1", "3");
    }

    [Fact]
    public void GetScopedHistory_ByQuestionType_Returns_Only_History_For_QuestionType()
    {
        _sut.Add(new HistoryItem(IntQuestion, Who, "1"));
        _sut.Add(new HistoryItem(IntQuestion, Why, "2"));
        _sut.Add(new HistoryItem(IntQuestion, Who, "3"));
        _sut.Add(new HistoryItem(IntQuestion, Why, "4"));
        _sut.Add(new HistoryItem(DecimalQuestion, Who, "5"));

        var scopedHistory = _sut.GetScopedHistory(HistoryType.ByQuestionType, IntQuestion, "NotRelevant");
        scopedHistory.Items.Should().BeEquivalentTo("1", "2", "3", "4");
    }

    [Fact]
    public void GetScopedHistory_NotFiltered_Returns_Only_History_For_QuestionType()
    {
        _sut.Add(new HistoryItem(IntQuestion, Who, "1"));
        _sut.Add(new HistoryItem(IntQuestion, Why, "2"));
        _sut.Add(new HistoryItem(IntQuestion, Who, "3"));
        _sut.Add(new HistoryItem(IntQuestion, Why, "4"));
        _sut.Add(new HistoryItem(DecimalQuestion, Who, "5"));

        var scopedHistory = _sut.GetScopedHistory(HistoryType.NotFiltered, "NotRelevant", "NotRelevant");
        scopedHistory.Items.Should().BeEquivalentTo("1", "2", "3", "4", "5");
    }

    [Fact]
    public void GetScopedHistory_When_Duplicate_Answers_Returns_Only_Last_Answer()
    {
        _sut.Add(new HistoryItem(IntQuestion, Who, "1"));
        _sut.Add(new HistoryItem(IntQuestion, Why, "5"));
        _sut.Add(new HistoryItem(IntQuestion, Who, "2"));
        _sut.Add(new HistoryItem(IntQuestion, Why, "2"));
        _sut.Add(new HistoryItem(DecimalQuestion, Who, "5"));

        var scopedHistory = _sut.GetScopedHistory(HistoryType.NotFiltered, "NotRelevant", "NotRelevant");
        scopedHistory.Items.Should().BeEquivalentTo("1", "2", "5");
    }
}