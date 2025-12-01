using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;
using System.Net.Http.Headers;

namespace AdventOfCode.Test
{
    public class UnitTests
    {
        //[Fact]
        public void DoPathFindingCorrectly()
        {
            var input = File.ReadAllText("TestData/maze1.txt");

            var grid = input.ToGrid();

            var start = new Point2(4, 1);
            var end = new Point2(1, 7);

            var paths = Graph.Implicit<Point2>(c => GetAdjacent(grid, c)).Bfs().AllPaths(start, end).ToList();

            Assert.Equal(3, paths.Count);
        }

        private IEnumerable<(Point2, Point2)> GetAdjacent(Grid2<char> grid, Point2 current)
        {
            foreach (var neighbor in current.GetNeighbors())
            {
                if (grid[neighbor] != '#')
                    yield return (current, neighbor);
            }
        }
    }
}
