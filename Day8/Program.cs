using System.Text.RegularExpressions;
Instruction ParseLine(string line)
{
    Regex regex = new(@"^([a-z]+)\s(inc|dec)\s(-?\d+)\sif\s([a-z]+)\s(==|!=|>=|<=|<|>)\s(-?\d+)$");
    Match result = regex.Match(line);

    return new Instruction(
        result.Groups[1].Value,
        (result.Groups[2].Value == "inc") ? IncDec.Increase : IncDec.Decrease,
        int.Parse(result.Groups[3].Value),
        result.Groups[4].Value,
        GetCompare(result.Groups[5].Value),
        int.Parse(result.Groups[6].Value)
    );
}

Compare GetCompare(string s)
{
    return s switch
    {
        "==" => Compare.Equal,
        "!=" => Compare.NotEqual,
        ">=" => Compare.GreaterOrEqual,
        "<=" => Compare.LessOrEqual,
        ">" => Compare.Greater,
        "<" => Compare.Less,
        _ => Compare.Equal,
    };
}

(int,Dictionary<string, int>) ExecuteInstructions(Instruction[] instructions)
{
    Dictionary<string, int> registers = [];
    int HighestValue = 0;

    foreach (Instruction instruction in instructions)
    {
        if (!registers.ContainsKey(instruction.Register))
        {
            registers[instruction.Register] = 0;
        }
        int compareReg = registers.ContainsKey(instruction.ConditionRegister) ? registers[instruction.ConditionRegister] : 0;

        bool execute = false;
        switch (instruction.ConditionCompare)
        {
            case Compare.Equal:
                execute = (compareReg == instruction.ConditionAmount); break;
            case Compare.NotEqual:
                execute = (compareReg != instruction.ConditionAmount); break;
            case Compare.GreaterOrEqual:
                execute = (compareReg >= instruction.ConditionAmount); break;
            case Compare.LessOrEqual:
                execute = (compareReg <= instruction.ConditionAmount); break;
            case Compare.Greater:
                execute = (compareReg > instruction.ConditionAmount); break;
            case Compare.Less:
                execute = (compareReg < instruction.ConditionAmount); break;
        }
        if (execute)
        {
            if (instruction.Action == IncDec.Increase)
            {
                registers[instruction.Register] += instruction.Amount;                
            }
            else
            {
                registers[instruction.Register] -= instruction.Amount;
            }
            HighestValue = Math.Max(HighestValue, registers[instruction.Register]);
        }
    }

    return (HighestValue, registers);
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

var instructions = lines.Select(ParseLine).ToArray();
var (highestValue,registers) = ExecuteInstructions(instructions);

Console.WriteLine("Part 1: {0}", registers.Values.Max());
Console.WriteLine("Part 2: {0}", highestValue);

enum IncDec
{
    Increase,
    Decrease,
}
enum Compare
{
    Equal,
    NotEqual,
    Greater,
    Less,
    GreaterOrEqual,
    LessOrEqual,
}

record Instruction(
    string Register,
    IncDec Action,
    int Amount,
    string ConditionRegister,
    Compare ConditionCompare,
    int ConditionAmount
    );
