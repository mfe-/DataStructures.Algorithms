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
        public IDictionary<Guid, IEdge> AStarManhattanDistanceDictionary()
        {
            IVertex goalToFind = null;

            IVertex VertexFactoryDouble(int x, int y)
            {
                var vertex = new Vertex<Point>();
                vertex.Value = new Point(x, y);
                return vertex;
            }
            var g = GraphExtensions.GenerateGridGraph(1024, 1024, VertexFactoryDouble, (lastVertex) => goalToFind = lastVertex, null, 0.1);
            Point goalPoint = ((IVertex<Point>)goalToFind).Value;
            Func<IVertex, double> funcManhattanDistanceHeuristic = new Func<IVertex, double>((vertex) =>
            {
                Point currentPoint = ((IVertex<Point>)vertex).Value;
                return Math.Abs(currentPoint.X - goalPoint.X) + Math.Abs(currentPoint.Y - goalPoint.Y);

            });
            return g.Start.AStar(goalToFind, funcManhattanDistanceHeuristic);
        }
        [Benchmark]
        public void AStarManhattanDistance()
        {
            AStarManhattanDistanceDictionary().Consume(consumer);
        }
    }
}
