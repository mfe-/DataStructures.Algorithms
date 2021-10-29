using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using DataStructures;
using DataStructures.Algorithms.Graph;
using static DataStructures.Algorithms.Graph.GraphExtensions;
using System;
using System.Drawing;

namespace Algorithms.Graph.Test
{
    public class GraphBenchmark
    {
        DataStructures.Graph graph1024;
        DataStructures.Graph graphAStar1024;
        IVertex lastVertexOfgraph1024;

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
            return graph1024.Start.DepthFirstSearchStack();
        }
        [Benchmark]
        public void DfsStack()
        {
            DfsStackEnumerable().Consume(consumer);
        }
        public IEnumerable<IVertex> BfsQueueEnumerable()
        {
            return graph1024.Start.BreadthFirstSearchQueue();
        }
        [Benchmark]
        public void BfsQueue()
        {
            BfsQueueEnumerable().Consume(consumer);
        }
        public IEnumerable<IEdge> DepthFirstSearchUndirectedEnumerable()
        {
            return graph1024.Start.DepthFirstSearch(graphIsDirected: false);
        }
        [Benchmark]
        public void DepthFirstSearchUndirected()
        {
            DepthFirstSearchUndirectedEnumerable().Consume(consumer);
        }

        [GlobalSetup]
        public void GlobalSetup()
        {
            graph1024 = GenerateGridGraph(1024, 1024);
            IVertex VertexFactoryDouble(int x, int y)
            {
                var vertex = new Vertex<Point>();
                vertex.Value = new Point(x, y);
                return vertex;
            }
            graphAStar1024 = GraphExtensions.GenerateGridGraph(1024, 1024, VertexFactoryDouble, (lastVertex) => lastVertexOfgraph1024 = lastVertex, null, 0.1);
        }

        public IDictionary<Guid, IEdge> AStarManhattanDistanceDictionary()
        {
            Point goalPoint = ((IVertex<Point>)lastVertexOfgraph1024).Value;
            Func<IVertex, double> funcManhattanDistanceHeuristic = new Func<IVertex, double>((vertex) =>
            {
                Point currentPoint = ((IVertex<Point>)vertex).Value;
                return Math.Abs(currentPoint.X - goalPoint.X) + Math.Abs(currentPoint.Y - goalPoint.Y);

            });
            return graphAStar1024.Start.AStar(lastVertexOfgraph1024, funcManhattanDistanceHeuristic);
        }
        [Benchmark]
        public void AStarManhattanDistance()
        {
            AStarManhattanDistanceDictionary().Consume(consumer);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            graph1024 = null;
            graphAStar1024 = null;
            lastVertexOfgraph1024 = null;
        }
    }
}
