``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1288 (21H1/May2021Update)
Intel Core i7-6650U CPU 2.20GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.100-rc.2.21505.57
  [Host]     : .NET 5.0.5 (5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET 5.0.5 (5.0.521.16609), X64 RyuJIT


```
|                     Method |       Mean |    Error |   StdDev |     Median |
|--------------------------- |-----------:|---------:|---------:|-----------:|
|                   DfsStack | 2,083.0 ms | 40.54 ms | 74.13 ms | 2,049.9 ms |
|                   BfsQueue | 1,516.5 ms | 29.54 ms | 23.06 ms | 1,509.2 ms |
| DepthFirstSearchUndirected | 3,110.9 ms | 59.93 ms | 85.95 ms | 3,072.9 ms |
|     AStarManhattanDistance |   124.1 ms |  1.25 ms |  1.17 ms |   124.1 ms |
