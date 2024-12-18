﻿var console = ConsoleFactory.Create();

var serviceProvider = GetServiceProvider(console);
using var scope = serviceProvider.CreateScope();

console.WriteHelpTextLines();

var apps = new[]
{
    typeof(MultiThreadedApp),
    typeof(LoggingApp),
    typeof(CancelApp),
    typeof(ReadMeApp),
    typeof(FlowApp),
    typeof(DemoApp),
    typeof(WipApp),
};

while (true)
{
    try
    {
        var appType = await console.AskForItem(
            "Which app to start?",
            apps,
            t => t.Name);

        var app = (IApp)scope.ServiceProvider.GetRequiredService(appType);
        await app.Run();
        console.WriteSuccessLine("Completed");
    }
    catch (TaskCanceledByF12Exception)
    {
        console.WriteErrorLine("Canceled By F12");
    }
}

static IServiceProvider GetServiceProvider(IConsole console)
    => new ServiceCollection()
        .AddSingleton(console)
        .AddLogging(c => c.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Error))
        .AddScoped<MultiThreadedApp>()
        .AddScoped<LoggingApp>()
        .AddScoped<CancelApp>()
        .AddScoped<ReadMeApp>()
        .AddScoped<FlowApp>()
        .AddScoped<DemoApp>()
        .AddScoped<WipApp>()
        .BuildServiceProvider();
