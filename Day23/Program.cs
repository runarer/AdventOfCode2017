string[] lines;

using (StreamReader reader = new(args[0]))
{
    try
    {
        string valueA = reader.ReadToEnd();
        lines = [.. valueA.Split(['\n']).Where(line => !string.IsNullOrEmpty(line)).Select(line => line.Trim())];
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
        return 1;
    }
}


Computer computer = new(lines);
computer.RunDebugMode(); ;
Console.WriteLine($"Part 1: {computer.MulInvoked}");

//computer.RunNormalMode();
//Console.WriteLine($"Part 2: {computer.FinalValue()}");

int nonPrimes = findNonPrimes();
Console.WriteLine($"Part 2: {nonPrimes}");

return 0;

int findNonPrimes()
{
    int h = 0;
    // the last prime we need to check is 349 as it is the last prime that
    // squared is smaller than the last number we need to check
    int[] primes = [2,   3,   5,   7,   11,  13,  17,  19,  23,  29,
                31,  37,  41,  43,  47,  53,  59,  61,  67,  71,
                73,  79,  83,  89 , 97,  101, 103, 107, 109, 113,
                127, 131, 137, 139, 149, 151, 157, 163, 167, 173,
                179, 181, 191, 193, 197, 199, 211, 223, 227, 229,
                233, 239, 241, 251, 257, 263, 269, 271, 277, 281,
                283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353]; 
    for (int number = 105700; number <= 122700; number += 17)
    {
        foreach (int prime in primes)
            if (number % prime == 0)
            {
                h++;
                break;
            }
    }
    return h;
}

public class Computer
{
    Dictionary<string, int> register = [];
    int instruction = 0;
    readonly Action[] instructions = [];

    public int MulInvoked = 0;

    public void TurnOffDebugMode() => register["a"] = 1;
    public int FinalValue() => register["h"];

    public Computer(string[] unparsedInstructions)
    {
        instructions = new Action[unparsedInstructions.Length];
        int i = 0;
        foreach (string line in unparsedInstructions)
        {
            string[] lineParts = line.Split(' ');
            Action action = () => Console.WriteLine("Something went wrong");
            switch (lineParts[0])
            {
                case "set":
                    // check if second argument is a number of label
                    if (int.TryParse(lineParts[2], out int setNumber))
                        action = () => Set(lineParts[1], setNumber);
                    else
                        action = () => Set(lineParts[1], lineParts[2]);
                    break;
                case "sub":
                    if (int.TryParse(lineParts[2], out int subNumber))
                        action = () => Sub(lineParts[1], subNumber);
                    else
                        action = () => Sub(lineParts[1], lineParts[2]);
                    break;
                case "mul":
                    if (int.TryParse(lineParts[2], out int mulNumber))
                        action = () => Mul(lineParts[1], mulNumber);
                    else
                        action = () => Mul(lineParts[1], lineParts[2]);
                    break;
                case "jnz":
                    if (int.TryParse(lineParts[2], out int jnzNumber2))
                        if (int.TryParse(lineParts[1], out int jnzNumber1))
                            action = () => Jnz(jnzNumber1, jnzNumber2);
                        else
                            action = () => Jnz(lineParts[1], jnzNumber2);
                    else
                        action = () => Jnz(lineParts[1], lineParts[2]);
                    break;
                default:
                    throw new Exception("Unsupported instruction: " + lineParts[0]);
            }
            instructions[i++] = action;
        }
    }
    public void Set(string s, int value)
    {
        register[s] = value;
        instruction++;
    }
    public void Set(string s1, string s2)
    {
        register[s1] = register[s2];
        instruction++;
    }
    public void Sub(string s, int value)
    {
        register[s] -= value;
        instruction++;
    }
    public void Sub(string s1, string s2)
    {
        register[s1] -= register[s2];
        instruction++;
    }
    public void Mul(string s, int value)
    {
        register[s] *= value;
        instruction++;
        MulInvoked++;
    }
    public void Mul(string s1, string s2)
    {
        register[s1] *= register[s2];
        instruction++;
        MulInvoked++;
    }
    public void Jnz(string s1, string s2)
    {
        if (register[s1] != 0)
            instruction += register[s2];
        else
            instruction++;
    }
    public void Jnz(string s, int value)
    {
        if (register[s] != 0)
            instruction += value;
        else
            instruction++;
    }
    public void Jnz(int v1, int v2)
    {
        if (v1 != 0)
            instruction += v2;
        else
            instruction++;
    }
    public void RunNormalMode()
    {
        register = new() { { "a", 1 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 }, { "h", 0 } };
        Run();
    }

    public void RunDebugMode()
    {
        register = new() { { "a", 0 }, { "b", 0 }, { "c", 0 }, { "d", 0 }, { "e", 0 }, { "f", 0 }, { "g", 0 }, { "h", 0 } };
        Run();
    }

    private void Run()
    {
        instruction = 0;
        while (instruction < instructions.Length)
        {
            instructions[instruction].Invoke();
        }
    }
}
/*
 For solving part 2

set b 57
set c b
jnz a 2 // I normal modus så hopper over neste
jnz 1 5 
mul b 100
sub b -100000
set c b
sub c -17000 // Kommer ikke tilbake hit. "Start verdiene" blir da b=105_700, c=122700 endres ikke senere
set f 1 // Retur her, siste -23
set d 2
set e 2 // Retur her, nest siste -13
set g d // retur her, første -8
mul g e
sub g b 
jnz g 2 // hvis g ikke er null
set f 0 // Dette er når g == b
sub e -1 
set g e
sub g b
jnz g -8 // Ser ut som om vi teller ned 
sub d -1
set g d
sub g b
jnz g -13
jnz f 2
sub h -1 // Eneste som endrer h, h vil være antall runder hvor f har blitt 0
set g b // denne og de tre neste, sjekker om b er lik c.
sub g c
jnz g 2 // b må være c, 122700
jnz 1 3 // Dette er exit
sub b -17 // Når b ikke er lik 122700 så legges til 17.
jnz 1 -23
 
 

Vi har tre runder, kan optimailsere vekk de to innerste. 
Eller bedre, forstå hva programmet gjør.
Jeg tror programmet finner ikke-primtall. f setter til 0 når det er to tall 
som ganget sammen blir tallet man jobber med i rekken. h økes hvis f har blitt
satt til 0. Dette vil ikke skje ved primtall.
 
*/
