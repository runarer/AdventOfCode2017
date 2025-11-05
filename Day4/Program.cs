bool AllUnique(string line)
{
    var words = line.Split(' ');
    foreach (var word in words)
    {
        if (words.Count(w => w == word) > 1)
        {
            return false;
        }
    }

    return true;
}

bool NoAnagrams(string line)
{
    var words = line.Split(' ')
        .Select( word => { 
            var chars = word.ToArray(); 
            Array.Sort(chars); 
            return new string(chars); }
        ).ToArray();

    for (int i = 0; i < words.Length - 1; i++)
    {        
        for (int j = i + 1; j < words.Length; j++)
        {
            if (words[i] == words[j]) return false;
        }
    }

    return true;
}

int validLines(List<string> lines, Func<string, bool> isValid)
{
    int validLines = 0;
    foreach (var line in lines)
    {
        if (isValid(line)) validLines++;
    }
    return validLines;
}


int? Part1(List<string> lines)
{
    return validLines(lines, AllUnique);
}

int? Part2(List<string> lines)
{
    return validLines(lines, NoAnagrams);
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

Console.WriteLine("Part 1: {0}", Part1(lines));
Console.WriteLine("Part 2: {0}", Part2(lines));