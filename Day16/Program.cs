string input = String.Empty;

try
{
    input = File.ReadAllText(args[0]);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}

(char, int, int)[] DanceMoves = ParseDanceMoves(input);

Console.WriteLine("Part 1: {0}", Part1(DanceMoves));
Console.WriteLine("Part 1: {0}", Part2(DanceMoves));
return 0;

(char, int, int)[] ParseDanceMoves(string input)
{
    return [.. input.Trim().Split(',').Select(ParseMove)];
}

(char, int, int) ParseMove(string move)
{
    char command = move[0];
    int first = 0;
    int second = 0;

    if (command == 's')
    {
        first = int.Parse(move[1..]);
    }

    if (command == 'x')
    {
        int[] fromTo = [.. move.Substring(1).Split('/').Select(int.Parse)];
        first = fromTo[0];
        second = fromTo[1];

    }
    if (command == 'p')
    {
        string[] fromTo = [.. move.Substring(1).Split('/')];
        first = fromTo[0][0];
        second = fromTo[1][0];
    }
    return (command, first, second);
}

/*
 Optimalization:
Can use an int[] were [0] is the position of 'a', this can remove the need for mod.
Can treat the input as a "programming language" and "compile" a program from it.

The pattern most likely repeats. Find how often. There's 20,922,789,888,000 permutations.
 */


string Part1((char, int, int)[] DanceMoves)
{
    char[] line = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p'];

    line = Dance(DanceMoves, line);

    return string.Join(String.Empty, line);
}
string Part2((char, int, int)[] DanceMoves)
{
    char[] line = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p'];
    int repeatsEvery = 0;
    int totalRuns = 1_000_000_000;

    // Find repeats
    for (int i = 0; i < totalRuns; i++)
    {
        line = Dance(DanceMoves, line);
        if (String.Join(String.Empty, line) == "abcdefghijklmnop")
        {
            repeatsEvery = i + 1;
            break;
        }
    }

    // Find results
    int left = totalRuns % repeatsEvery;
    for (int i = 0; i < left; i++)
    {
        line = Dance(DanceMoves, line);
        if (String.Join(String.Empty, line) == "abcdefghijklmnop")
        {
            repeatsEvery = i + 1;
            break;
        }
    }

    return string.Join(String.Empty, line);
}


char[] Dance((char, int, int)[] danceMoves, char[] line)
{
    char[] temp = new char[line.Length];

    foreach (var (command, first, second) in danceMoves)
    {
        if (command == 's')
        {
            for (int i = 0; i < line.Length; i++)
            {
                temp[(i + first) % line.Length] = line[i];
            }

            (line, temp) = (temp, line);
        }
        else if (command == 'x')
        {
            (line[second], line[first]) = (line[first], line[second]);
        }
        else if (command == 'p')
        {
            int from = Array.IndexOf(line, (char)first);
            int to = Array.IndexOf(line, (char)second);
            (line[to], line[from]) = (line[from], line[to]);
        }
    }

    return line;
}

