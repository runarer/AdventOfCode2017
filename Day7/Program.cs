using System.Text.RegularExpressions;

(string, int, string[]) ParseLine(string line)
{
    Regex regex = new(@"^([a-z]+)\s\((\d+)\)(?:\s->\s(.*))?$");

    Match result = regex.Match(line);

    return (result.Groups[1].Value, int.Parse(result.Groups[2].Value), [.. result.Groups[3].Value.Split([' ', ','], StringSplitOptions.RemoveEmptyEntries)]);
}

//string Part1(List<string> lines)
//{
//    HashSet<string> programs = [];
//    HashSet<string> linkedPrograms = [];

//    foreach (string line in lines)
//    {
//        var (program, _, linked) = ParseLine(line);
//        programs.Add(program);
//        linkedPrograms.UnionWith(linked);
//    }

//    return programs.Except(linkedPrograms).First();
//}

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
            programs[child].AddParent(programs[parent]);
        }
    }

    // Find root
    ProgramNode root = programs[programs.Keys.Except(linkedPrograms).First()];

    root.UpdateWeights();

    return root;
}

int? Part2(ProgramNode root)
{
    foreach( ProgramNode child in root.SubPrograms)
    {
        if (child.TotalWeight != root.CorrectWeight)
            return Part2(child);
    }

    return root.Weight + (root.Parent.CorrectWeight - root.TotalWeight);
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

ProgramNode root = BuildTree(lines);
Console.WriteLine("Part 1: {0}", root.Name);
Console.WriteLine("Part 2: {0}", Part2(root));


class ProgramNode(string name, int weight)
{
    public string Name { get; private set; } = name;
    public int Weight { get; private set; } = weight;
    public int TotalWeight { get; private set; } = weight;
    public List<ProgramNode> SubPrograms { get; private set; } = new List<ProgramNode>();
    public ProgramNode? Parent { get; private set; } = null;
    public int CorrectWeight { get; private set; } = weight;

    public void AddLink(ProgramNode linked)
    {
        SubPrograms.Add(linked);
    }

    public void AddParent(ProgramNode parent)
    {
        Parent = parent;
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