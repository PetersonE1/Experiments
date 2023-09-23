using AsyncTesting;
using BenchmarkDotNet.Running;

ConsoleTest test = new ConsoleTest();

Task read = Task.CompletedTask;
while (true)
{
    if (read.IsCompleted)
        _ = test.ReadConsole();
    await test.WriteToConsole();
}