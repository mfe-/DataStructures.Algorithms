using DataStructures;
using Xunit;
using Xunit.Abstractions;

namespace Algorithms.Graph.Test
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
            DataStructures.Graph g = GraphExtensions.GenerateGridGraph(4, 4, (i) => new Vertex());

            GraphExtensions.Serialize(g.Start, "muh");

        }
    }
}
