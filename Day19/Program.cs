
string[] lines = [];
try
{
    lines = File.ReadAllLines(args[0]);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}

char[,] map = ParseLines(lines);
int start = FindStart(map);

Console.WriteLine($"Part 1: {Part1(map, start)}");
Console.WriteLine($"Part 2: {Part2()}");

return 0;

string? Part1(char[,] map, int start)
{
    List<char> letters = [];

    (int y, int x) = (0, start);
    Direction dir = Direction.South;
    char current = map[0, start];

    while (current != ' ')
    {
        if (current == '+')
        {
            // Change direction
            dir = NewDirection(map, x, y, dir);
        }
        if (Char.IsLetter(current))
        {
            // We have a letter, append
            letters.Add(current);
        }
        (y, x) = FindNext(x, y, dir);
        current = map[y, x];
    }
    return string.Join(string.Empty, letters); ;
}
int? Part2()
{
    return null;
}

char[,] ParseLines(string[] lines)
{
    char[,] map = new char[lines.Length, lines[0].Length];

    for (int i = 0; i < map.GetLength(0); i++)
        for (int j = 0; j < map.GetLength(1); j++)
        {
            map[i, j] = lines[i][j];
        }

    return map;
}

int FindStart(char[,] map)
{
    int start = 0;

    for (int i = 0; i < map.GetLength(1); i++)
    {
        if (map[0, i] == '|')
        {
            start = i;
            break;
        }
    }

    return start;
}

(int, int) FindNext(int x, int y, Direction dir)
{
    (int y, int x)[] next = [(-1, 0), (0, 1), (1, 0), (0, -1)];
    return (y + next[(int)dir].y, x + next[(int)dir].x);
}


Direction NewDirection(char[,] map, int x, int y, Direction cur)
{
    Direction newDir;
    if (cur == Direction.North || cur == Direction.South)
    {
        if (x == map.GetLength(1) - 1) newDir = Direction.West;
        else if (x == 0) newDir = Direction.East;
        else if (map[y, x - 1] == '-' || Char.IsLetter(map[y, x - 1])) newDir = Direction.West;
        else newDir = Direction.East;
    }
    else
    {
        if (y == map.GetLength(0) - 1) newDir = Direction.North;
        else if (y == 0) newDir = Direction.South;
        else if (map[y - 1, x] == '|' || Char.IsLetter(map[y - 1, x])) newDir = Direction.North;
        else newDir = Direction.South;
    }

    return newDir;
}

enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3,
}