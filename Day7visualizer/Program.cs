using System.Text.RegularExpressions;

(string, int, string[]) ParseLine(string line)
{
    Regex regex = new(@"^([a-z]+)\s\((\d+)\)(?:\s->\s(.*))?$");

    Match result = regex.Match(line);

    return (result.Groups[1].Value, int.Parse(result.Groups[2].Value), [.. result.Groups[3].Value.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries)]);
}

ProgramNode BuildTree(List<string> lines)
{
    Dictionary<string, ProgramNode> programs = [];
    HashSet<string> linkedPrograms = [];
    List<KeyValuePair<string, string[]>> parentChildren = [];

    // Parse lines and create nodes
    foreach (string line in lines)
    {
        var (program, weight, linked) = ParseLine(line);
        programs[program] = new ProgramNode(program, weight);
        parentChildren.Add(new KeyValuePair<string, string[]>(program, linked));
        linkedPrograms.UnionWith(linked);
    }

    // Build tree
    foreach (var (parent, children) in parentChildren)
    {
        foreach (var child in children)
        {
            programs[parent].AddLink(programs[child]);
        }
    }

    ProgramNode root = programs[programs.Keys.Except(linkedPrograms).First()];
    root.UpdateWeights();

    // Find and return root
    return root;
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

void PrintTree(ProgramNode program, int depth, bool balanced = false)
{
    Console.ForegroundColor = (balanced) ? ConsoleColor.Green : ConsoleColor.Red;
    Console.WriteLine($"{new string('|', depth)} {program.Name}({program.Weight} - {program.TotalWeight})");

    foreach (ProgramNode child in program.SubPrograms)
    {
        PrintTree(child, depth + 1, (child.TotalWeight == program.CorrectWeight));
    }
}

ProgramNode root = BuildTree(lines);
ConsoleColor originalForegroundColor = Console.ForegroundColor;
PrintTree(root, 0);
Console.ForegroundColor = originalForegroundColor;



class ProgramNode(string name, int weight)
{
    public string Name { get; private set; } = name;
    public int Weight { get; private set; } = weight;
    public int TotalWeight { get; private set; } = weight;
    public List<ProgramNode> SubPrograms { get; private set; } = new List<ProgramNode>();
    public int CorrectWeight { get; private set; } = weight;

    public void AddLink(ProgramNode linked)
    {
        SubPrograms.Add(linked);
    }

    public int UpdateWeights()
    {
        TotalWeight += SubPrograms.Sum(x => x.UpdateWeights());
        if (SubPrograms.Count == 2)
        {
            CorrectWeight = SubPrograms[0].TotalWeight;
        }
        else if (SubPrograms.Count > 2)
        {
            if (SubPrograms[0].TotalWeight == SubPrograms[1].TotalWeight)
            {
                CorrectWeight = SubPrograms[0].TotalWeight;
            }
            else
            {
                CorrectWeight = SubPrograms[2].TotalWeight;
            }
        }

        return TotalWeight;
    }
}