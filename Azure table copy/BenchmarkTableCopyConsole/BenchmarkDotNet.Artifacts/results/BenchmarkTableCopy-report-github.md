``` ini

BenchmarkDotNet=v0.10.4, OS=Windows 10.0.14393
Processor=Intel Core i7 CPU M 620 2.67GHz, ProcessorCount=4
Frequency=2597473 Hz, Resolution=384.9896 ns, Timer=TSC
dotnet cli version=1.0.0
  [Host]          : .NET Core 4.6.25009.03, 64bit RyuJIT [AttachedDebugger]
  FastAndDirtyJob : .NET Core 4.6.25009.03, 64bit RyuJIT

Job=FastAndDirtyJob  LaunchCount=1  RunStrategy=ColdStart  
TargetCount=5  WarmupCount=5  

```
 |          Method |       Mean |        Error |     StdDev |
 |---------------- |-----------:|-------------:|-----------:|
 | BenchBatchSize1 |  55.1805 s |    40.5314 s |  10.5279 s |
 | BenchBatchSize2 |  44.1022 s |    20.3510 s |   5.2861 s |
 | BenchBatchSize3 | 224.0705 s | 1,489.6789 s | 386.9385 s |
