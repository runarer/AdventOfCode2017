int[][] lines = [];
string[] deliminators = ["<->", ","];
try
{
    lines = [..File.ReadAllLines(args[0])
                   .Select(x => x.Split(deliminators, StringSplitOptions.RemoveEmptyEntries)
                                 .Skip(1)
                                 .Select(int.Parse)
                                 .ToArray()
                           )
            ];
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}


Console.WriteLine($"Part 1: {Part1(lines)}");
Console.WriteLine($"Part 1: {Part2(lines)}");
return 0;

void FindGroup(int[][] lines, int start, bool[] visited)
{
    Queue<int> queue = new();

    queue.Enqueue(start);

    while (queue.Count > 0)
    {
        int current = queue.Dequeue();
        visited[current] = true;

        foreach (int next in lines[current])
        {
            if (visited[next]) continue;
            queue.Enqueue(next);
        }
    }
}
int? Part1(int[][] lines) {
    bool[] visited = new bool[lines.Length];
    FindGroup(lines, 0, visited);
    return visited.Count(x => x);
}
int? Part2(int[][] lines)
{
    bool[] visited = new bool[lines.Length];
    int groups = 0;

    for (int i = 0; i < lines.Length; i++)
    {
        if (visited[i]) continue;
        FindGroup(lines, i, visited);
        groups++;
    }

    return groups;
}


