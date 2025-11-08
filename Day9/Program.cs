int Part1(string stream)
{
    int totalScore = 0;
    int currentScore = 0;
    bool garbage = false;

    for (int i = 0; i < stream.Length; i++)
    {
        if (garbage)
        {
            if (stream[i] == '!')
            {
                i++;
                continue;
            }
            if (stream[i] == '>')
            {
                garbage = false;
                continue;
            }
        }
        else
        {
            if (stream[i] == '<')
            {
                garbage = true;
                continue;
            }
            if (stream[i] == '{')
            {
                currentScore++;
                continue;
            }
            if (stream[i] == '}')
            {
                totalScore += currentScore;
                currentScore--;
                continue;
            }
        }
    }
    return totalScore;
}

int Part2(string stream)
{
    bool garbage = false;
    int totalGarbage = 0;

    for (int i = 0; i < stream.Length; i++)
    {
        if (garbage)
        {
            if (stream[i] == '!')
            {
                i++;
                continue;
            }
            if (stream[i] == '>')
            {
                garbage = false;
                continue;
            }
            totalGarbage++;
        }
        else
        {
            if (stream[i] == '<')
            {
                garbage = true;
                continue;
            }
        }
    }
    return totalGarbage;
}

string stream = String.Empty;
using (StreamReader sr = new(args[0]))
{
    while (!sr.EndOfStream)
    {
        string? line = sr.ReadLine();
        if (line is not null)
        {
            stream = line;
        }
    }
}

Console.WriteLine("Part 1: {0}", Part1(stream));
Console.WriteLine("Part 2: {0}", Part2(stream));