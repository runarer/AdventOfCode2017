/* Visual Studio auto complete guessed the solution, not fun! */

int? Part1(List<int> instructions)
{
    List<int> instCopy = [.. instructions];

    int instructionPointer = 0;
    int steps = 0;
    while (instructionPointer >= 0 && instructionPointer < instCopy.Count)
    {
        int jump = instCopy[instructionPointer];
        instCopy[instructionPointer] += 1;
        instructionPointer += jump;
        steps++;
    }

    return steps;
}

int? Part2(List<int> instructions)
{
    List<int> instCopy = [.. instructions];

    int instructionPointer = 0;
    int steps = 0;
    while (instructionPointer >= 0 && instructionPointer < instCopy.Count)
    {
        int jump = instCopy[instructionPointer];
        if (jump >= 3)
        {
            instCopy[instructionPointer] -= 1;
        }
        else
        {
            instCopy[instructionPointer] += 1;
        }
        instructionPointer += jump;
        steps++;
    }
    return steps;
}


List<int> instructions = [];
using (StreamReader sr = new StreamReader(args[0]))
{
    while (!sr.EndOfStream)
    {
        string? line = sr.ReadLine();
        if (line is not null)
        {
            instructions.Add(int.Parse(line));
        }
    }
}

Console.WriteLine("Part 1: {0}", Part1(instructions));
Console.WriteLine("Part 2: {0}", Part2(instructions));