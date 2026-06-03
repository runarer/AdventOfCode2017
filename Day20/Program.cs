using System.Text.RegularExpressions;

string[] lines = [];
try
{
    lines = File.ReadAllLines(args[0]);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}

var particles = lines.Select(ParseLine);

Console.WriteLine($"Part 1: {Part1(map, start)}");
Console.WriteLine($"Part 2: {Part2()}");




return 0;


Point ParseLine(string line)
{
    var result = MyRegex().Match(line.Trim());
    return new Point(
        new Vector(int.Parse(result.Groups[1].Value), int.Parse(result.Groups[2].Value), int.Parse(result.Groups[3].Value)),
        new Vector(int.Parse(result.Groups[4].Value), int.Parse(result.Groups[5].Value), int.Parse(result.Groups[6].Value)),
        new Vector(int.Parse(result.Groups[7].Value), int.Parse(result.Groups[8].Value), int.Parse(result.Groups[9].Value))
    );
}

record Vector(int X, int Y, int Z);

record Point(Vector Position, Vector Speed, Vector Acceleration);

partial class Program
{
    [GeneratedRegex(@"p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>")]
    private static partial Regex MyRegex();
}