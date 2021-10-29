``` ini

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1288 (21H1/May2021Update)
Intel Core i7-6650U CPU 2.20GHz (Skylake), 1 CPU, 4 logical and 2 physical cores
.NET SDK=6.0.100-rc.2.21505.57
  [Host]     : .NET 5.0.5 (5.0.521.16609), X64 RyuJIT
  DefaultJob : .NET 5.0.5 (5.0.521.16609), X64 RyuJIT


```
|                     Method |       Mean |    Error |    StdDev |
|--------------------------- |-----------:|---------:|----------:|
|                   DfsStack | 2,080.1 ms | 40.82 ms |  59.83 ms |
|                   BfsQueue | 1,500.7 ms | 27.75 ms |  25.96 ms |
| DepthFirstSearchUndirected | 3,149.0 ms | 62.47 ms | 162.37 ms |
|     AStarManhattanDistance |   121.8 ms |  1.19 ms |   0.93 ms |
