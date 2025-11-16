using Instruction = (InstructionSet, (int?, Register?), (int?, Register?)?);

Instruction[] instructions = [];

try
{
    string[] lines = File.ReadAllLines(args[0]);
    instructions = [.. lines.Select(ParseLine)];
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    return 1;
}

Console.WriteLine("Part 1: {0}", Part1(instructions));
Console.WriteLine("Part 1: {0}", Part2(instructions));

return 0;

Instruction ParseLine(string line)
{
    string[] res = line.Split(' ');

    // Instruction
    InstructionSet instruction = GetInstruction(res[0]);

    // First argument
    (int?, Register?) arg1;
    if (int.TryParse(res[1], out int value))
    {
        arg1 = (value, null);
    }
    else
    {
        arg1 = (null, GetRegister(res[1]));
    }

    // Second arguments, may not exist.
    (int?, Register?)? arg2 = null;
    if (instruction != InstructionSet.snd && instruction != InstructionSet.rcv)
    {
        int valueInt;

        if (int.TryParse(res[2], out valueInt))
        {
            arg2 = (valueInt, null);
        }
        else
        {
            arg2 = (null, GetRegister(res[2]));
        }
    }

    return (instruction, arg1, arg2);
}

long? Part1(Instruction[] instructions)
{
    long[] registers = [0, 0, 0, 0, 0];
    long? sound = null;
    long pointer = 0;
    long? recoveredSound = null;

    while (pointer >= 0 && pointer < instructions.Length)
    {        
        var (ins, arg1, arg2) = instructions[pointer];

        switch (ins)
        {
            case InstructionSet.set:
                registers[(int)arg1.Item2] = arg2?.Item1 ?? registers[(int)arg2?.Item2];
                break;
            case InstructionSet.mul:
                registers[(int)arg1.Item2] *= arg2?.Item1 ?? registers[(int)arg2?.Item2];
                break;
            case InstructionSet.jgz:                
                if ((arg1.Item2 == null && arg1.Item1 > 0) || (arg1.Item2 != null && registers[(int)arg1.Item2] > 0))
                {
                    pointer--;
                    pointer += arg2?.Item1 ?? registers[(int)arg2?.Item2];
                }
                break;
            case InstructionSet.add:
                registers[(int)arg1.Item2] += arg2?.Item1 ?? registers[(int)arg2?.Item2];
                break;
            case InstructionSet.mod:
                registers[(int)arg1.Item2] %= arg2?.Item1 ?? registers[(int)arg2?.Item2];
                break;
            case InstructionSet.snd:
                sound = arg1.Item1 ?? registers[(int)arg1.Item2];
                break;
            case InstructionSet.rcv:
                if (registers[(int)arg1.Item2] != 0)
                {
                    recoveredSound = sound;
                    pointer = -10_000;
                }                
                break;
        }        
            pointer++;
    }

    return recoveredSound;
}

int? Part2(Instruction[] instructions)
{
    return null;
}

InstructionSet GetInstruction(string ins)
{
    return ins switch
    {
        "set" => InstructionSet.set,
        "mul" => InstructionSet.mul,
        "jgz" => InstructionSet.jgz,
        "add" => InstructionSet.add,
        "mod" => InstructionSet.mod,
        "snd" => InstructionSet.snd,
        "rcv" => InstructionSet.rcv,
        _ => throw new ArgumentException($"Unknown instruction: {ins}")
    };
}

Register GetRegister(string reg)
{
    return reg switch
    {
        "a" => Register.a,
        "b" => Register.b,
        "f" => Register.f,
        "i" => Register.i,
        "p" => Register.p,
        _ => throw new ArgumentException($"Unknows register:{reg}"),
    };
}

enum InstructionSet
{
    set,
    mul,
    jgz,
    add,
    mod,
    snd,
    rcv,
}
enum Register
{
    a = 0,
    b = 1,
    f = 2,
    i = 3,
    p = 4,
}
