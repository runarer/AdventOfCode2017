string line = String.Empty;
try
{
    line = File.ReadAllLines(args[0]).First();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    return 1;
}

KnotHash kh = new();

bool[][] squares = [..Enumerable.Range(0, 128)
                                   .Select(i => $"{line}-{i}")
                                   .Select(kh.GetKnotHash)
                                   .Select(StringToBool)];


Console.WriteLine($"Part 1: {UsedSquares(squares)}");
Console.WriteLine($"Part 2: {Regions(squares)}");
return 0;

int UsedSquares(bool[][] squares)
{
    return squares.Sum(y => y.Sum(x => x ? 1 : 0));
}

int Regions(bool[][] squares)
{
    int regions = 0;

    for (int i = 0; i < squares.Length; i++)
        for (int j = 0; j < squares[i].Length; j++)
        {
            if (squares[i][j])
            {
                regions++;
                MarkRegionAsFound(squares, i, j);
            }
        }

    return regions;
}

void MarkRegionAsFound(bool[][] squares, int i, int j)
{
    Queue<(int, int)> partOfRegin = new();
    partOfRegin.Enqueue((i, j));
    while (partOfRegin.Count > 0)
    {
        var (line, col) = partOfRegin.Dequeue();

        // Allready visited?
        if (!squares[line][col]) continue;

        // Visiting
        squares[line][col] = false;

        // Look for neighbors
        if (line < 127 && squares[line + 1][col])
        {
            partOfRegin.Enqueue((line + 1, col));
        }
        if (col < 127 && squares[line][col + 1])
        {
            partOfRegin.Enqueue((line, col + 1));
        }
        if (line > 0 && squares[line - 1][col])
        {
            partOfRegin.Enqueue((line - 1, col));
        }
        if (col > 0 && squares[line][col - 1])
        {
            partOfRegin.Enqueue((line, col - 1));
        }
    }
}

bool[] StringToBool(string line)
{
    return [.. line.SelectMany(CharToBool)];
}

static bool[] CharToBool(char hex)
{
    return hex switch
    {
        '0' => [false, false, false, false],
        '1' => [false, false, false, true],
        '2' => [false, false, true, false],
        '3' => [false, false, true, true],
        '4' => [false, true, false, false,],
        '5' => [false, true, false, true],
        '6' => [false, true, true, false],
        '7' => [false, true, true, true],
        '8' => [true, false, false, false],
        '9' => [true, false, false, true],
        'a' => [true, false, true, false],
        'b' => [true, false, true, true],
        'c' => [true, true, false, false],
        'd' => [true, true, false, true],
        'e' => [true, true, true, false],
        'f' => [true, true, true, true],
        _ => throw new NotImplementedException()
    };

}


class KnotHash()
{
    int[] list = [.. Enumerable.Range(0, 256)];
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

    private int[] SparseHash(int[] lengths, int rounds = 1)
    {
        for (int i = 0; i < rounds; i++)
            Hash(lengths);
        return list;
    }

    private int[] DenseHash()
    {
        int[] dense = new int[16];

        for (int i = 0; i < dense.Length; i++)
        {
            dense[i] = list[i * dense.Length];
            for (int j = 1; j < dense.Length; j++)
            {
                dense[i] ^= list[i * dense.Length + j];
            }
        }
        return dense;
    }

    public string GetKnotHash(string line)
    {
        int[] inputAsASCII = [.. line.Select(x => (int)x), 17, 31, 73, 47, 23];

        SparseHash(inputAsASCII, 64);
        int[] denseHash = DenseHash();
        string[] hexValues = [.. denseHash.Select(x => x.ToString("X2").ToLower())];

        // Reset object
        list = [.. Enumerable.Range(0, 256)];
        current = 0;
        skip = 0;

        return String.Join(String.Empty, hexValues);
    }
}
