/* Another way is to use an array, size defined at start (2018).
 * In the array we keep tuples of (index_next,index_prev) and the
 * array index is the value.
 * This way each time we add we just need to add one and update two. 
 * This solution was faster but still to slow.
 * It's propably a calculation thing.
 */

int steps = 0;
try
{
    steps = int.Parse(File.ReadAllText(args[0]).Trim());
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    return 1;
}




/* TEST */
//steps = 3;

Console.WriteLine($"Part 1: {Part1(steps,2018)}");
Console.WriteLine($"Part 1: {Part2(steps,50_000_000)}");
return 0;


int Part1(int steps, int ValuesToAdd)
{
    int[,] list = new int[ValuesToAdd, 2];
    list[0, 0] = 1;
    list[0, 1] = 1;
    list[1, 0] = 0;
    list[1, 1] = 0;
    int currentNode = 1;
    int currentSize = 2;

    while (currentSize < ValuesToAdd)
    {
        int movesForward = steps % currentSize;
        while (movesForward > 0)
        {
            currentNode = list[currentNode, 1];
            movesForward--;
        }

        list[currentSize, 0] = currentNode;
        list[currentSize, 1] = list[currentNode, 1];
        list[list[currentSize, 0], 1] = currentSize;
        list[list[currentSize, 1], 0] = currentSize;
        currentNode = currentSize;
        currentSize++;        
    }

    return list[2017, 1];
}

int Part2(int steps,int ValuesToAdd)
{
    int[,] list = new int[ValuesToAdd, 2];
    list[0, 0] = 1;
    list[0, 1] = 1;
    list[1, 0] = 0;
    list[1, 1] = 0;
    int currentNode = 1;
    int currentSize = 2;

    while (currentSize < ValuesToAdd)
    {
        int movesForward = steps % currentSize;
        while (movesForward > 0)
        {
            currentNode = list[currentNode, 1];
            movesForward--;
        }

        list[currentSize, 0] = currentNode;
        list[currentSize, 1] = list[currentNode, 1];
        list[list[currentSize, 0], 1] = currentSize;
        list[list[currentSize, 1], 0] = currentSize;
        currentNode = currentSize;
        currentSize++;
    }

    return list[0, 1];
}

// This is brute force and not a good solution.

//int? Part1(int steps)
//{
//    RoundList rl = new(steps);
//    while (rl.Count < ValuesToAdd)
//    {
//        rl.AddValue();
//    }

//    return rl.PeakAtNext();
//}


// Takes close to four minutes to finish.
//int? Part2(int steps)
//{
//    RoundList rl = new(steps);
//    while (rl.Count < 50_000_000)
//    {
//        rl.AddValue();
//    }

//    return rl.Root.Next.Value;
//}

//class RoundList
//{

//    public int Count { get; set; } = 1;
//    public Node Root = new(0);

//    private Node _currentNode;
//    private int _currentValue = 0;
//    private int _steps = 0;

//    public RoundList(int steps)
//    {
//        _steps = steps;
//        _currentNode = Root;
//        _currentNode.Next = new(1);
//        _currentNode.Prev = _currentNode.Next;
//        _currentNode.Next.Next = _currentNode;
//        _currentNode.Next.Prev = _currentNode;
//        _currentNode = _currentNode.Next;
//        _currentValue = 1;
//        Count++;
//    }

//    public void AddValue()
//    {
//        int movesForward = _steps % Count;
//        while (movesForward > 0)
//        {
//            movesForward--;
//            _currentNode = _currentNode.Next;
//        }

//        Count++;
//        _currentValue++;
//        Node newNode = new(_currentValue);
//        newNode.Next = _currentNode.Next;
//        newNode.Prev = _currentNode;
//        _currentNode = newNode;
//        newNode.Next.Prev = newNode;
//        newNode.Prev.Next = newNode;
//    }

//    public int PeakAtNext()
//    {
//        if (_currentNode.Next == null) throw new Exception("Missing Next node!");
//        return _currentNode.Next.Value;
//    }
//}

//class Node(int value)
//{
//    public int Value { get; private set; } = value;
//    public Node? Next { get; set; } = null;
//    public Node? Prev { get; set; } = null;

//}