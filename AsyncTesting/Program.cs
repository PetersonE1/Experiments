using AsyncTesting;
using BenchmarkDotNet.Running;

var summary = BenchmarkRunner.Run<SequenceTest>();