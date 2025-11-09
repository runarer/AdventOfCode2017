
string line = string.Empty;
try
{
    line = File.ReadAllText(args[0]);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}

Direction[] directions = [.. line.Trim().Split(',').Select(GetDirection)];

Console.WriteLine($"Part 1: {Part1(directions)}");
Console.WriteLine($"Part 2: {Part2(directions)}");

return 0;
int? Part1(Direction[] directions)
{ 
    // Walk
    Coordinate start = new(0, 0, 0);
    foreach (Direction direction in directions)
    {
        start = start.Step(direction);
    }

    int[] ints = [Math.Abs(start.q), Math.Abs(start.s), Math.Abs(start.r)];

    return ints.Max();
}

int? Part2(Direction[] directions)
{
    int maxDist = 0;

    // Walk
    Coordinate start = new(0, 0, 0);
    foreach (Direction direction in directions)
    {
        start = start.Step(direction);
        int[] ints = [maxDist,Math.Abs(start.q), Math.Abs(start.s), Math.Abs(start.r)];
        maxDist = ints.Max();
    }

    return maxDist;
}

Direction GetDirection(string direction) => direction switch
{
    "n" => Direction.North,
    "nw" => Direction.NorthWest,
    "ne" => Direction.NorthEast,
    "s" => Direction.South,
    "sw" => Direction.SouthWest,
    "se" => Direction.SouthEast,
    _ => throw new NotImplementedException(),
};


record Coordinate(int q, int s, int r)
{
    public Coordinate Step(Direction direction) =>

         direction switch
         {
             Direction.North => new Coordinate(q, s + 1, r - 1),
             Direction.NorthWest => new Coordinate(q - 1, s + 1, r),
             Direction.NorthEast => new Coordinate(q + 1, s, r - 1),
             Direction.South => new Coordinate(q, s - 1, r + 1),
             Direction.SouthWest => new Coordinate(q - 1, s, r + 1),
             Direction.SouthEast => new Coordinate(q + 1, s - 1, r),
             _ => throw new NotImplementedException(),
         };

}

enum Direction
{
    North,
    NorthWest,
    NorthEast,
    South,
    SouthWest,
    SouthEast,
}