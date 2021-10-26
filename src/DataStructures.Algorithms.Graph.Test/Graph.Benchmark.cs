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
        public DataStructures.Graph GenerateGridGraph(int i, int j)
        {
            return GraphExtensions.GenerateGridGraph(i, j, VertexFactoryGeneric);
        }
        IVertex VertexFactoryGeneric(int x, int y)
        {
            return new Vertex<int>() { Weighted = x };
        }
        public IEnumerable<IVertex> DfsStackEnumerable()
        {
            return GenerateGridGraph(1024, 1024).Start.DepthFirstSearchStack();
        }
        [Benchmark]
        public void DfsStack()
        {
            DfsStackEnumerable().Consume(consumer);
        }
        public IEnumerable<IVertex> BfsQueueEnumerable()
        {
            return GenerateGridGraph(1024, 1024).Start.BreadthFirstSearchQueue();
        }
        [Benchmark]
        public void BfsQueue()
        {
            BfsQueueEnumerable().Consume(consumer);
        }
        public IEnumerable<IEdge> DepthFirstSearchUndirectedEnumerable()
        {
            return GenerateGridGraph(1024, 1024).Start.DepthFirstSearch(graphIsDirected: false);
        }
        [Benchmark]
        public void DepthFirstSearchUndirected()
        {
            DepthFirstSearchUndirectedEnumerable().Consume(consumer);
        }

    }
}
