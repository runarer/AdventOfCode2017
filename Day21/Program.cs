
string[] lines;

(int[], int[]) Parse2x2Rule(string line)
{
    List<int> top = [];
    List<int> bottom = [];

    // ^([\.#]{2})\/([\.#]{2}) => ([\.#]{3})\/([\.#]{3})\/([\.#]{3})
}

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


        
var twoByTwoLines = lines.Take(6).Select().ToArray();
var threeByThreeLines = lines.Skip(6).ToArray();


return 0;