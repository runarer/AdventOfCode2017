int? Part1()
{
    return null;
}

int? Part2()
{
    return null;
}


List<string> lines = [];
using (StreamReader sr = new StreamReader(args[0]))
{
    while (!sr.EndOfStream)
    {
        string? line = sr.ReadLine();
        if (line != null)
        {
            lines.Add(line);
        }
    }
}

Console.WriteLine("Part 1: {0}", Part1());
Console.WriteLine("Part 2: {0}", Part2());