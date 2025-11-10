string[] lines;
try
{
    lines = File.ReadAllLines(args[0]);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}

int[] Firewall = CreateFirewall(ParseLines(lines));
//int[] Firewall = CreateFirewall([(0, 3), (1, 2), (4, 4), (6, 4)]);

Console.WriteLine($"Part 1: {FindSeverity(Firewall)}");
Console.WriteLine($"Part 2: {FindDelay(Firewall)}");


return 0;

(int, int) ParseLine(string line)
{
    string[] split = line.Split(": ");
    return (int.Parse(split[0]), int.Parse(split[1]));
}

List<(int, int)> ParseLines(string[] lines)
{
    List<(int, int)> depthRange = [];

    foreach (string line in lines)
    {
        depthRange.Add(ParseLine(line));
    }

    return depthRange;
}

int[] CreateFirewall(List<(int, int)> lookup)
{
    int[] firewall = new int[lookup.Last().Item1 + 1];

    foreach (var (depth, range) in lookup)
    {
        firewall[depth] = range;
    }

    return firewall;
}

int FindSeverity(int[] firewall)
{
    int severity = 0;

    for (int depth = 0; depth < firewall.Length; depth++)
    {
        // No scanner
        if (firewall[depth] == 0) continue;

        // Are we caught?
        int scannerPresentAt = 2 * firewall[depth] - 2;
        if (depth % scannerPresentAt == 0)
            severity += depth * firewall[depth];
    }

    return severity;
}

bool Caught(int[] firewall, int delay)
{
    bool caugth = false;

    for (int depth = 0; depth < firewall.Length; depth++)
    {
        // No scanner
        if (firewall[depth] == 0) continue;

        // Are we caught?
        int scannerPresentAt = 2 * firewall[depth] - 2;
        if ((depth + delay) % scannerPresentAt == 0)
        {
            caugth = true; break;
        }
    }

    return caugth;
}

int FindDelay(int[] firewall)
{
    int delay = 4;

    while (Caught(firewall, delay)) delay++;

    return delay;
}