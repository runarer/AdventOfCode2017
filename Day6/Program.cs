(int, int[]) FindLoop(int[] memory)
{
    int sum = memory.Sum();

    int[] memCopy = [.. memory];
    int cycles = 0;

    HashSet<string> seenConfigurations = new();

    string config = string.Join(" ", memCopy);
    while (!seenConfigurations.Contains(config))
    {
        seenConfigurations.Add(config);

        int maxValue = memCopy.Max();
        int indexLargest = Array.IndexOf(memCopy, maxValue);

        int addOneTo = maxValue % memCopy.Length;
        int addToEach = maxValue / memCopy.Length;

        memCopy[indexLargest] = 0;
        for (int i = 0; i < memCopy.Length; i++)
        {
            memCopy[i] += addToEach;
            if (i > indexLargest && i <= indexLargest + addOneTo)
            {
                memCopy[i]++;
            }

            if (indexLargest + addOneTo >= memCopy.Length && i <= (indexLargest + addOneTo) % memCopy.Length)
            {
                memCopy[i]++;
            }
        }

        cycles++;
        config = string.Join(" ", memCopy);
    }

    return (cycles,memCopy);
}


int? Part1(int[] memory)
{
    return FindLoop(memory).Item1;
}

int? Part2(int[] memory)
{
    int[] loopStart = FindLoop(memory).Item2;
    return FindLoop(loopStart).Item1;
}


int[] memory = [];
using (StreamReader sr = new(args[0]))
{
    while (!sr.EndOfStream)
    {
        string? line = sr.ReadLine();
        if (line is not null)
        {
            memory = [.. line.Split('\t').Select(int.Parse)];
        }
    }
}

Console.WriteLine("Part 1: {0}", Part1(memory));
Console.WriteLine("Part 2: {0}", Part2(memory));

