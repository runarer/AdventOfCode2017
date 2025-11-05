int? Part1(List<int[]> rows)
{
    return rows.Aggregate(0, (acc,cur) => acc + cur.Max() - cur.Min());
}

int? Part2(List<int[]> rows)
{
    int checksum = 0;
    foreach (var row in rows)
    {
        for (int i = 0; i < row.Length-1; i++)
        {
            for (int j = i + 1; j < row.Length; j++)
            {
                if (row[j] % row[i] == 0)
                    checksum += row[j] / row[i];
                else if (row[i] % row[j] == 0)
                    checksum += row[i] / row[j];
            }
        }
    }
    return checksum;
}

List<int[]> rows = [];
using (StreamReader sr = new (args[0]))
{
    while (!sr.EndOfStream)
    {
        string? line = sr.ReadLine();
        if (line is not null)
        {
            //Console.WriteLine(line);
            rows.Add([.. line.Split('\t').Select(i => int.Parse(i))]);
        }
    }
}


Console.WriteLine("Part 1: {0}", Part1(rows));
Console.WriteLine("Part 2: {0}", Part2(rows));
