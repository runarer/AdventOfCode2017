
using System.Text.RegularExpressions;
using System.Diagnostics;

string[] lines;

using (StreamReader reader = new(args[0]))
{
    try
    {
        string valueA = reader.ReadToEnd();
        lines = valueA.Split(['\n']);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
        return 1;
    }
}
        
var twoByTwoLines = lines.Take(6).Select(Parse2x2Rule).ToArray();
var threeByThreeLines = lines
    .Skip(6)
    .Where(line => !String.IsNullOrWhiteSpace(line))
    .Select(Parse3x3Rule)
    .ToArray();

Dictionary<string, string>  twoToThree= [];
foreach (var (numbers,replacement) in twoByTwoLines)
{
    foreach (var number in numbers)
    {
        twoToThree[number] = replacement;
    }
}

Dictionary<string, string> threeToFour= [];
foreach (var (numbers, replacement) in threeByThreeLines)
{
    foreach (var number in numbers)
    {
        threeToFour[number] = replacement;
    }
}

Dictionary<string, Dictionary<int, long>> memo = [];

// Start timing part 1
var sw1 = Stopwatch.StartNew();
long part1 = calculateNonEmptySpaces(".#...####", 5);
sw1.Stop();

// Start timing part 2
var sw2 = Stopwatch.StartNew();
long part2 = calculateNonEmptySpaces(".#...####", 18);
sw2.Stop();

Console.WriteLine($"Part 1: {part1} (took {sw1.Elapsed.TotalMilliseconds} ms)");
Console.WriteLine($"Part 2: {part2} (took {sw2.Elapsed.TotalMilliseconds} ms)");

return 0;

long calculateNonEmptySpaces(string pattern, int iterations)
{
    if (pattern.Length != 9) throw new IndexOutOfRangeException();

    // Check if we have already done this calculation
    if(memo.TryGetValue(pattern, out var result))
    {
        if (result.TryGetValue(iterations, out var ret))
            return ret;
    }

    char[] image = new char[9*9];

    insertImagePart(image, pattern, 0, 3);
    if (iterations <= 0) return image.Sum(c  => c == '#' ? 1 :0);

    // replace with a 4x4 pattern
    insertImagePart(image, threeToFour[pattern], 0, 4);
    iterations--;
    if (iterations == 0) return image.Sum(c => c == '#' ? 1 : 0);

    string[] imageParts = new string[4];
    // Divide into 4 2x2
    int part = 0;
    for (int row = 0; row < 2; row++)
        for (int col = 0; col < 2; col++)
            imageParts[part++] = string.Concat(image.AsSpan(2*col+(row*9*2),2), image.AsSpan(2*col+9+(row*9*2),2));

    // insert new pattern
    part = 0;
    for (int row = 0; row < 2; row++)
        for (int col = 0; col < 2; col++)
        {
            var replacement = twoToThree[imageParts[part++]];
            int start = row * 27 + col * 3;
            insertImagePart(image,replacement, start, 3);
        }

    iterations--;
    if (iterations <= 0) return image.Sum(c => c == '#' ? 1 : 0);

    // Divide into 9 2x2
    imageParts = new string[9];
    part = 0;
    for (int row = 0; row < 3; row++)
        for (int col = 0; col < 3; col++)
            imageParts[part++] = string.Concat(image.AsSpan(2 * col + (row * 9 * 2), 2), image.AsSpan(2 * col + 9 + (row * 9 * 2), 2));


    // insert new pattern
    part = 0;
    for (int row = 0; row < 3; row++)
        for (int col = 0; col < 3; col++)
        {
            var replacement = twoToThree[imageParts[part++]];
            int start = row * 27 + col * 3;
            insertImagePart(image, replacement, start, 3);
        }

    iterations--;
    if (iterations <= 0) return image.Sum(c => c == '#' ? 1 : 0);


    // Divide into 9 3x3
    imageParts = new string[9];
    part = 0;
    for (int row = 0; row < 3; row++)
        for (int col = 0; col < 3; col++)
            imageParts[part++] = string.Concat(image.AsSpan(3 * col + (row * 9 * 3), 3), image.AsSpan(3 * col + 9 + (row * 9 * 3), 3), image.AsSpan(3 * col + 18 + (row * 9 * 3), 3));

    // call recursive on each image part
    long[] values = [.. imageParts.Select(p => calculateNonEmptySpaces(p, iterations))];
    for (int i = 0; i < values.Length; i++)
    {
        if (memo.TryGetValue(imageParts[i], out var value))
            value[iterations] = values[i];
        else
        {
            memo[imageParts[i]] = [];
            memo[imageParts[i]][iterations] = values[i];
        }
    }

    return values.Sum();
}


void insertImagePart(char[] image, string pattern, int start, int size)
{
    for (int i = 0; i < size; i++)
        for (int j = 0; j < size; j++)
        {
            int pixel = start + i + 9 * j;
            image[pixel] = pattern[i + (size * j)];
        }

}

(string[], string) Parse2x2Rule(string line)
{
    var result = Regex2x2().Match(line.Trim());
    string replacement = result.Groups[3].Value + result.Groups[4].Value + result.Groups[5].Value;

    string[] lookups = new string[8];
    int i = 0;
    foreach (var variation in rotateAndFlip2x2(result.Groups[1].Value + result.Groups[2].Value))
    {
        lookups[i++] = variation;
    }

    return (lookups, replacement);
}

(string[], string) Parse3x3Rule(string line)
{
    var result = Regex3x3().Match(line.Trim());

    string replacement = result.Groups[4].Value + result.Groups[5].Value + result.Groups[6].Value + result.Groups[7].Value;

    string[] lookups = new string[8];
    int i = 0;
    foreach (var variation in rotateAndFlip3x3(result.Groups[1].Value + result.Groups[2].Value + result.Groups[3].Value))
    {
        lookups[i++] = variation;
    }

    return (lookups, replacement);
}

string[] rotateAndFlip2x2(string line)
{
    char a = line[0];
    char b = line[1];
    char c = line[2];
    char d = line[3];

    return [
        new([a, b, c, d]), 
        new([c, a, d, b]), 
        new([d, c, b, a]), 
        new([b, d, a, c]), 
        new([b, a, d, c]), 
        new([a, c, b, d]), 
        new([b, a, d, c]), 
        new([c, d, a, b])];
}

string[] rotateAndFlip3x3(string line)
{
    // Orginalt
    // abc
    // def
    // ghi

    char a = line[0];
    char b = line[1];
    char c = line[2];
    char d = line[3];
    char e = line[4];
    char f = line[5];
    char g = line[6];
    char h = line[7];
    char i = line[8];

    return [
        new([a, b, c, d, e, f, g, h, i]), 
        new([g, d, a, h, e, b, i, f, c]), 
        new([i, h, g, f, e, d, c, b, a]), 
        new([c, f, i, b, e, h, a, d, g]),        
        new([c, b, a, f, e, d, i, h, g]), 
        new([g, h, i, d, e, f, a, b, c]),
        new([a, d, g, b, e, h, c, f, i]),
        new([i, f, c, h, e, b, g, d, a])];
}

partial class Program
{
    [GeneratedRegex(@"^([\.#]{2})\/([\.#]{2}) => ([\.#]{3})\/([\.#]{3})\/([\.#]{3})")]
    private static partial Regex Regex2x2();

    [GeneratedRegex(@"^([#\.]{3})\/([#\.]{3})\/([#\.]{3}) => ([#\.]{4})\/([#\.]{4})\/([#\.]{4})\/([#\.]{4})$")]
    private static partial Regex Regex3x3();
}