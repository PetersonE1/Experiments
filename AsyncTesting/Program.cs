using AsyncTesting;
using (FileStream fs = new FileStream("outputLog.txt", FileMode.Create, FileAccess.Write))
{
    using StreamWriter writer = new StreamWriter(fs);
    Console.SetOut(writer);
    Console.WriteLine("Test: i = 0; i < iteration; i++; t += i; Console.WriteLine(t)");
    
    Console.WriteLine("\nIterations: 1000");
    Console.WriteLine("Running synchronous test");
    Console.WriteLine(Test1.Run(1000) + " ms");
    Console.WriteLine("-----------------------------------");
    Console.WriteLine("Running awaited asynchronous test");
    Console.WriteLine(await Test2.Run(1000) + " ms");
    Console.WriteLine("-----------------------------------");
    Console.WriteLine("Running true asynchronous test");
    Console.WriteLine(Test3.Run(1000) + " ms");
    
    Console.WriteLine("\nIterations: 10000");
    Console.WriteLine("Running synchronous test");
    Console.WriteLine(Test1.Run(10000) + " ms");
    Console.WriteLine("-----------------------------------");
    Console.WriteLine("Running awaited asynchronous test");
    Console.WriteLine(await Test2.Run(10000) + " ms");
    Console.WriteLine("-----------------------------------");
    Console.WriteLine("Running true asynchronous test");
    Console.WriteLine(Test3.Run(10000) + " ms");
    
    Console.WriteLine("\nIterations: 100000");
    Console.WriteLine("Running synchronous test");
    Console.WriteLine(Test1.Run(100000) + " ms");
    Console.WriteLine("-----------------------------------");
    Console.WriteLine("Running awaited asynchronous test");
    Console.WriteLine(await Test2.Run(100000) + " ms");
    Console.WriteLine("-----------------------------------");
    Console.WriteLine("Running true asynchronous test");
    Console.WriteLine(Test3.Run(100000) + " ms");
}


