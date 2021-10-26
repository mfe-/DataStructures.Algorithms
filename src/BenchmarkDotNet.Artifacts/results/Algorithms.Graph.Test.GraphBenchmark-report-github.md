``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1288 (21H1/May2021Update)
Intel Core i7-6650U CPU 2.20GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.100-rc.2.21505.57
  [Host]     : .NET 5.0.5 (5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET 5.0.5 (5.0.521.16609), X64 RyuJIT


```
|                          Method |     Mean |    Error |   StdDev |
|-------------------------------- |---------:|---------:|---------:|
|                        DfsStack |  3.257 s | 0.0289 s | 0.0241 s |
|                        BfsQueue |  2.836 s | 0.0278 s | 0.0260 s |
|      DepthFirstSearchUndirected | 11.441 s | 0.0650 s | 0.0576 s |
| DepthFirstSearchUndirected_Sse2 |  4.344 s | 0.0654 s | 0.0546 s |
