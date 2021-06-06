using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using DataStructures;
using DataStructures.Algorithms.Graph;
using static DataStructures.Algorithms.Graph.GraphExtensions;

namespace Algorithms.Graph.Test
{
    public class GraphBenchmark
    {
        private readonly Consumer consumer = new Consumer();
        public DataStructures.Graph GenerateGridGraph()
        {
           return GraphExtensions.GenerateGridGraph(1000, 1000, VertexFactoryGeneric);
        }
        IVertex VertexFactoryGeneric(int x, int y)
        {
            return new Vertex<int>() { Weighted = x };
        }
        public IEnumerable<IVertex> DfsStackEnumerable()
        {
            return GenerateGridGraph().Start.DepthFirstSearchStack();
        }
        [Benchmark]
        public void DfsStack()
        {
            DfsStackEnumerable().Consume(consumer);
        }
        public IEnumerable<IVertex> BfsQueueEnumerable()
        {
            return GenerateGridGraph().Start.BreadthFirstSearchQueue();
        }
        [Benchmark]
        public void BfsQueue()
        {
            BfsQueueEnumerable().Consume(consumer);
        }
    }
}
