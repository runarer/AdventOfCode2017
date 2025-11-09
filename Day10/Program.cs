string line = string.Empty;
try
{
    line = File.ReadAllText(args[0]);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}

int Part1(string line)
{
    int[] lenghts = [.. line.Split(',').Select(int.Parse)];
    int[] list = new SparseKnotHash().SparseHash(lenghts);
    return list[0] * list[1];
}

string? Part2(string line)
{
    int[] inputAsASCII = [.. line.Select(x => (int)x), 17, 31, 73, 47, 23];

    SparseKnotHash hash = new SparseKnotHash();
    _ = hash.SparseHash(inputAsASCII, 64);
    int[] denseHash = hash.DenseHash();
    string[] hexValues = [..denseHash.Select(x => x.ToString("X2").ToLower())];

    return String.Join(String.Empty,hexValues);
}

Console.WriteLine($"Part 1: {Part1(line)}");
//Console.WriteLine($"Part 2 TEST: {Part2("".Trim())}");
//Console.WriteLine($"Part 2 TEST: {Part2("AoC 2017".Trim())}");
//Console.WriteLine($"Part 2 TEST: {Part2("1,2,3".Trim())}");
//Console.WriteLine($"Part 2 TEST: {Part2("1,2,4".Trim())}");
Console.WriteLine($"Part 2: {Part2("212,254,178,237,2,0,1,54,167,92,117,125,255,61,159,164".Trim())}");

return 0;

class SparseKnotHash(int listLength = 256)
{
    int[] list = [.. Enumerable.Range(0, listLength)];
    int current = 0;
    int skip = 0;

    private void Hash(int[] lengths)
    {
        foreach (int length in lengths)
        {
            // Reverse length elements
            if (current + length < list.Length)
            {
                Array.Reverse(list, current, length);
            }
            else
            {
                for (int i = 0; i < length / 2; i++)
                {
                    int from = (current + i) % list.Length;
                    int to = (from + length - 1 - 2 * i) % list.Length;
                    int Temp = list[to];
                    list[to] = list[from];
                    list[from] = Temp;
                }
            }

            // Move current length + skip
            current = (current + length + skip) % list.Length;

            // Increase skip
            skip++;
        }
    }

    public int[] SparseHash(int[] lengths, int rounds = 1)
    {
        for (int i = 0; i < rounds; i++)
            Hash(lengths);
        return list;
    }

    public int[] DenseHash()
    {
        int[] dense = new int[16];

        for(int i = 0; i< dense.Length; i++)
        {
            dense[i] = list[i * dense.Length];
            for (int j = 1; j < dense.Length; j++)
            {
                dense[i] ^= list[i * dense.Length + j];
            }
        }
        return dense;
    }
}
