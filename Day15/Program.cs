int FirstValueA = 0;
int FirstValueB = 0;

using (StreamReader reader = new(args[0]))
{
    try
    {
        string valueA = reader.ReadLine() ?? throw new Exception("Couldn't read first line");
        string valueB = reader.ReadLine() ?? throw new Exception("Couldn't read second line");
        FirstValueA = int.Parse(valueA.Split(' ').Last());
        FirstValueB = int.Parse(valueB.Split(' ').Last());
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}

Console.WriteLine("Part 1: {0}", Part1(FirstValueA, FirstValueB));
Console.WriteLine("Part 2: {0}", Part2(FirstValueA, FirstValueB));

int? Part1(int firstValueA, int firstValueB)
{
    int matchingPairs = 0;

    long generatorA = firstValueA;
    long generatorB = firstValueB;

    for (int i = 0; i < 40_000_000; i++)
    {
        generatorA = (generatorA * 16807) % 2147483647;
        generatorB = (generatorB * 48271) % 2147483647;

        ushort partA = (ushort)generatorA;
        ushort partB = (ushort)generatorB;
        if (partA == partB) { matchingPairs++; }
    }

    return matchingPairs;
}

int? Part2(int firstValueA, int firstValueB)
{
    int matchingPairs = 0;

    long generatorA = firstValueA;
    long generatorB = firstValueB;

    for (int i = 0; i < 5_000_000; i++)
    {
        do
        {
            generatorA = (generatorA * 16807) % 2147483647;
        } while (generatorA % 4 != 0);

        do
        {
            generatorB = (generatorB * 48271) % 2147483647;
        } while (generatorB % 8 != 0);


        ushort partA = (ushort)generatorA;
        ushort partB = (ushort)generatorB;
        if (partA == partB) { matchingPairs++; }
    }

    return matchingPairs;
}