int? Part1(int number)
{
    int numbers = 1;
    int spirals = 0;
    while (numbers < number)
    {
        spirals++;
        numbers += 8 * spirals;
    }

    int stepsBack = numbers - number;
    int fromCorner = spirals > 0 ? stepsBack % (spirals * 2) : 0;

    return spirals + spirals - fromCorner;
}

int? Part2(int number)
{
    int curDirection = 0; // 0 down, 1 right, 2 up, 3 left
    var curPosition = (0, 0);
    int curNumber = 1;

    Dictionary<(int, int), int> squares = [];
    

    while (curNumber < number)
    {
        squares.Add(curPosition, curNumber);

        (int, int)[] leftHands = { (0, 1), (1, 0), (0, -1), (-1, 0) };
        (int, int)[] moves = { (-1, 0), (0, 1), (1, 0), (0, -1) };
        (int, int) leftHand = (curPosition.Item1 + leftHands[curDirection].Item1, curPosition.Item2 + leftHands[curDirection].Item2);
        // check lefthand
        if (squares.GetValueOrDefault(leftHand, 0) == 0)
        {   // Turn and move
            curPosition = leftHand;
            curDirection = (curDirection + 1) % 4;
        }
        else
        { // Just Move
            curPosition = (curPosition.Item1 + moves[curDirection].Item1, curPosition.Item2 + moves[curDirection].Item2);
        }

        // Calculate value
        (int, int)[] neighbors = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };
        curNumber = 0;
        foreach(var neighbor in neighbors)
        {
            var checkSquare = (curPosition.Item1 + neighbor.Item1, curPosition.Item2 + neighbor.Item2);
            curNumber += squares.GetValueOrDefault(checkSquare, 0);
            
        }
    }

    return curNumber;
}

int number;

using (StreamReader sr = new StreamReader(args[0]))
{
    string? line = sr.ReadLine();
    bool isNumber = int.TryParse(line, out number);
}

Console.WriteLine("Part 1: {0}", Part1(number));
Console.WriteLine("Part 2: {0}", Part2(number));