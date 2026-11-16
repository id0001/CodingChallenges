using CodingChallenge.Utilities;
using CodingChallenge.Utilities.Attributes;
using CodingChallenge.Utilities.Collections.Graphs;
using CodingChallenge.Utilities.Extensions;

namespace AdventOfCode2018.Challenges;

[Challenge(22)]
public class Challenge22
{
    [Part(1, "8681")]
    public string Part1(string input)
    {
        var (depth, target) = ParseInput(input);
        var grid = new Grid2<int>(target.Y + 1, target.X + 1);
        for (var y = 0; y < grid.RowCount; y++)
        {
            for (var x = 0; x < grid.ColumnCount; x++)
            {
                var geologicalIndex = (x, y) switch
                {
                    (0, 0) => 0,
                    var (tx, ty) when tx == target.X && ty == target.Y => 0,
                    (0, _) => y * 48271,
                    (_, 0) => x * 16807,
                    _ => grid[y, x - 1] * grid[y - 1, x]
                };

                grid[y, x] = (geologicalIndex + depth) % 20183;
            }
        }

        return grid.Values.Sum(v => v % 3).ToString();
    }

    [Part(2, "1070")]
    public string Part2(string input)
    {
        var (depth, target) = ParseInput(input);
        var grid = CreateGrid(depth, target);

        var (path, cost) = Graph
            .Implicit<State, int>(n => GetAdjacent(grid, n))
            .Dijkstra()
            .ShortestPath(new State(Point2.Zero, EquipedTool.Torch), n => n.Location == target && n.Equiped == EquipedTool.Torch);

        return cost.ToString();
    }

    private static IEnumerable<(State, State, int)> GetAdjacent(Grid2<TerrainType> grid, State current)
    {
        var type = grid[current.Location];
        var bounds = grid.Bounds;

        if (current.Equiped != EquipedTool.Torch && grid[current.Location] != TerrainType.Wet)
            yield return (current, current with { Equiped = EquipedTool.Torch }, 7);

        if (current.Equiped != EquipedTool.ClimbingGear && grid[current.Location] != TerrainType.Narrow)
            yield return (current, current with { Equiped = EquipedTool.ClimbingGear }, 7);

        if (current.Equiped != EquipedTool.Neither && grid[current.Location] != TerrainType.Rocky)
            yield return (current, current with { Equiped = EquipedTool.Neither }, 7);

        foreach (var neighbor in current.Location.GetNeighbors().Where(bounds.Contains))
        {
            switch (grid[neighbor])
            {
                case TerrainType.Rocky when current.Equiped != EquipedTool.Neither:
                    yield return (current, current with { Location = neighbor }, 1);
                    break;
                case TerrainType.Wet when current.Equiped != EquipedTool.Torch:
                    yield return (current, current with { Location = neighbor }, 1);
                    break;
                case TerrainType.Narrow when current.Equiped != EquipedTool.ClimbingGear:
                    yield return (current, current with { Location = neighbor }, 1);
                    break;
                default:
                    break;
            }
        }
    }

    private static Grid2<TerrainType> CreateGrid(int depth, Point2 target)
    {
        var grid = new Grid2<int>(target.Y * 5, target.X * 5);
        for (var y = 0; y < grid.RowCount; y++)
        {
            for (var x = 0; x < grid.ColumnCount; x++)
            {
                var geologicalIndex = (x, y) switch
                {
                    (0, 0) => 0,
                    var (tx, ty) when tx == target.X && ty == target.Y => 0,
                    (0, _) => y * 48271,
                    (_, 0) => x * 16807,
                    _ => grid[y, x - 1] * grid[y - 1, x]
                };

                grid[y, x] = (geologicalIndex + depth) % 20183;
            }
        }

        var terrain = new Grid2<TerrainType>(grid.RowCount, grid.ColumnCount);
        foreach (var (p, v) in grid)
            terrain[p] = (TerrainType)(v % 3);

        return terrain;
    }

    private static (int, Point2) ParseInput(string input)
    {
        var (depth, x, y) = input.Extract<int, int, int>(@$"depth: (\d+){Environment.NewLine}target: (\d+),(\d+)");
        return (depth, new Point2(x, y));
    }

    private record State(Point2 Location, EquipedTool Equiped);

    private enum EquipedTool
    {
        Neither,
        Torch,
        ClimbingGear
    }


    private enum TerrainType
    {
        Rocky = 0,
        Wet = 1,
        Narrow = 2
    }
}
