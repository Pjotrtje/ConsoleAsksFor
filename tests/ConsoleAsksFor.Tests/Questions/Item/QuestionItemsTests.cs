namespace ConsoleAsksFor.Tests;

public class QuestionItemsTests
{
    [Fact]
    public void CreateWithoutEscapedSplitter_With_Splitter_In_Items_Does_Not_Fix_And_Had_No_Warnings()
    {
        var items = new[] { "Pre|Post" };
        var questionItems = QuestionItems.CreateWithoutEscapedSplitter(items);
        questionItems.Warnings().Should().BeEmpty();

        questionItems.Should().ContainSingle().Which.Display.Should().Be("Pre|Post");
    }

    [Fact]
    public void CreateWithEscapedSplitter_With_Splitter_In_Items_Fixes_And_Shows_Warning()
    {
        var items = new[] { "Pre|Post" };
        var questionItems = QuestionItems.CreateWithEscapedSplitter(items);
        questionItems.Warnings().Should().BeEquivalentTo(
            "One or more items contained splitter char |, those chars are replaced with [PIPE].",
            "Listed above is applied to to display value, question result is not adjusted. So what you see is not what you get...");

        questionItems.Should().ContainSingle().Which.Display.Should().Be("Pre[PIPE]Post");
    }

    [Fact]
    public void Create_With_Trailing_Or_Leading_In_Items_Fixes_And_Shows_Warning()
    {
        var items = new[] { " A" };
        var questionItems = QuestionItems.CreateWithoutEscapedSplitter(items);
        questionItems.Warnings().Should().BeEquivalentTo(
            "One or more items contained trailing/leading spaces, those items have been trimmed.",
            "Listed above is applied to to display value, question result is not adjusted. So what you see is not what you get...");

        questionItems.Should().ContainSingle().Which.Display.Should().Be("A");
    }

    [Fact]
    public void Create_With_Whitespace_In_Items_Fixes_And_Shows_Warning()
    {
        var items = new[] { "Pre\t\n\f\v\rPost" };
        var questionItems = QuestionItems.CreateWithoutEscapedSplitter(items);
        questionItems.Warnings().Should().BeEquivalentTo(
            "One or more items contained non-visible whitespace, which has been made visible.",
            "Listed above is applied to to display value, question result is not adjusted. So what you see is not what you get...");

        questionItems.Should().ContainSingle().Which.Display.Should().Be(@"Pre\t\n\f\v\rPost");
    }

    [Fact]
    public void Create_With_Non_Printable_Chars_In_Items_Fixes_And_Shows_Warning()
    {
        var items = new[] { "Pre\0Post" };
        var questionItems = QuestionItems.CreateWithoutEscapedSplitter(items);
        questionItems.Warnings().Should().BeEquivalentTo(
            "One or more items contained not printable chars, those chars are removed.",
            "Listed above is applied to to display value, question result is not adjusted. So what you see is not what you get...");

        questionItems.Should().ContainSingle().Which.Display.Should().Be(@"PrePost");
    }

    [Fact]
    public void Create_With_Empty_String_In_Items_Fixes_And_Shows_Warning()
    {
        var items = new[] { "" };
        var questionItems = QuestionItems.CreateWithoutEscapedSplitter(items);
        questionItems.Warnings().Should().BeEquivalentTo(
            "One item was empty and replaced with <EMPTY>.",
            "Listed above is applied to to display value, question result is not adjusted. So what you see is not what you get...");

        questionItems.Should().ContainSingle().Which.Display.Should().Be(@"<EMPTY>");
    }
}