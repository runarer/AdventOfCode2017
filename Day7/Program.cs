using System.Text.RegularExpressions;

(string, int, string[]) ParseLine(string line)
{
    Regex regex = new(@"^([a-z]+)\s\((\d+)\)(?:\s->\s(.*))?$");

    Match result = regex.Match(line);

    return (result.Groups[1].Value, int.Parse(result.Groups[2].Value), [.. result.Groups[3].Value.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries)]);
}


string Part1(List<string> lines)
{
    HashSet<string> programs = [];
    HashSet<string> linkedPrograms = [];

    foreach (string line in lines)
    {
        var (program, _, linked) = ParseLine(line);
        programs.Add(program);
        linkedPrograms.UnionWith(linked);
    }

    return programs.Except(linkedPrograms).First();
}

int? Part2(List<string> lines, string root)
{
    Dictionary<string, int> programs = [];
    Dictionary<string, string[]> links = [];

    foreach (string line in lines)
    {
        var (program, weight, link) = ParseLine(line);
        programs[program] = weight;
        if (link != null)
            links[program] = link;
    }

    return null;
}


List<string> lines = [];
using (StreamReader sr = new(args[0]))
{
    while (!sr.EndOfStream)
    {
        string? line = sr.ReadLine();
        if (line is not null)
        {
            lines.Add(line);
        }
    }
}

string root = Part1(lines);
Console.WriteLine("Part 1: {0}", root);
Console.WriteLine("Part 2: {0}", Part2(lines, root));
