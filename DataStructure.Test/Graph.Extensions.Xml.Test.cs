using Algorithms.Graph;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace DataStructures.Test
{
    public class GraphExtensionsXmlTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public GraphExtensionsXmlTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void SaveXmlShould_contain_all_vertices_and_edges()
        {
            //the graph to serialize
            Graph g = GraphExtensions.GenerateGridGraph(4, 4, (i) => new Vertex());

            GraphExtensions.Serialize(g.Start, "muh");

        }
    }
}
