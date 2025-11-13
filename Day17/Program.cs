/* Another way is to use an array, size defined at start (2018).
 * In the array we keep tuples of (index_next,index_prev) and the
 * array index is the value.
 * This way each time we add we just need to add one and update two. 
 */

int ValuesToAdd = 2018;
int steps = 0;
try
{
    steps = int.Parse(File.ReadAllText(args[0]).Trim());
} catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    return 1;
}

/* TEST */
//steps = 3;

Console.WriteLine($"Part 1: {Part1(steps)}");
Console.WriteLine($"Part 1: {Part2(steps)}");
return 0;

int? Part1(int steps)
{
    RoundList rl = new(steps);
    while( rl.Count < ValuesToAdd )
    {
        rl.AddValue();
    }
    
    return rl.PeakAtNext();
}

// This is brute force and not a good solution.
// Takes close to four minutes to finish.
int? Part2(int steps)
{
    RoundList rl = new(steps);
    while (rl.Count < 50_000_000)
    {
        rl.AddValue();
    }

    return rl.Root.Next.Value;
}

class RoundList
{
    
    public int Count { get; set; } = 1;
    public Node Root = new(0);
    
    private Node _currentNode;
    private int _currentValue = 0;
    private int _steps = 0;

    public RoundList(int steps)
    {
        _steps = steps;
        _currentNode = Root;
        _currentNode.Next = new(1);
        _currentNode.Prev = _currentNode.Next;
        _currentNode.Next.Next = _currentNode;
        _currentNode.Next.Prev = _currentNode;
        _currentNode = _currentNode.Next;
        _currentValue = 1;
        Count++;
    }

    public void AddValue()
    {
        int movesForward = _steps % Count;
        while (movesForward > 0)
        {
            movesForward--;
            _currentNode = _currentNode.Next;
        }

        Count++;
        _currentValue++;
        Node newNode = new(_currentValue);
        newNode.Next = _currentNode.Next;
        newNode.Prev = _currentNode;
        _currentNode = newNode;        
        newNode.Next.Prev = newNode;
        newNode.Prev.Next = newNode;
    }

    public int PeakAtNext()
    {
        if(_currentNode.Next == null) throw new Exception("Missing Next node!");
        return _currentNode.Next.Value;
    }
}

class Node(int value)
{
    public int Value { get; private set; } = value;
    public Node? Next { get; set; } = null;
    public Node? Prev { get; set; } = null;

}