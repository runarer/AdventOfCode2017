/* Denne oppgaven kan brukes for å vise bruk av enum og switch. */

string[] lines;

using (StreamReader reader = new(args[0]))
{
    try
    {
        string valueA = reader.ReadToEnd();
        lines = [..valueA.Split(['\n']).Where(line => !string.IsNullOrEmpty(line)).Select(line =>line.Trim())];
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
        return 1;
    }
}

int mapWidth = lines[0].Length;
int mapHeight = lines.Length; // We have an empty line at the end.

Console.WriteLine($"H: {mapHeight} W: {mapWidth}");

// The map is infinite so we need to keep track of infeced squares and not the whole map.
// It's the only information we need.
List<(int, int)> infected = [];


// The virus starts in the center so we need to orient the map after that.
// Center is 0,0
int y = (mapHeight - 1) / 2;
for (int row = 0; row < mapHeight; row++,y--)
{
    int x = -1 * ((mapWidth - 1) / 2);
    for (int col = 0; col < mapWidth; col++,x++)
    {
        if (lines[row][col] == '#')
            infected.Add((x, y));
    }
}

int infections = runBurst(10_000);
Console.WriteLine($"Part 1 {infections}");

int advancedInfections = runEvolvedVirus(10_000_000);
Console.WriteLine($"Part 2 {advancedInfections}");

return 0;


// The virus moves in burst so lets simulate those
int runBurst(int bursts)
{
    List<(int, int)> tempInfected = [.. infected];
    (int, int) currentNode = (0, 0);
    Direction currentDirection = Direction.Up;
    int infections = 0;

    for (int burst = 0; burst < bursts; burst++)
    {
        if (tempInfected.Contains(currentNode))
        {
            currentDirection = turnRight(currentDirection);
            tempInfected.Remove(currentNode);
        }
        else
        {
            currentDirection = turnLeft(currentDirection);
            tempInfected.Add(currentNode);
            infections++;
        }
        currentNode = move(currentNode.Item1, currentNode.Item2, currentDirection);
    }
    return infections;
}


int runEvolvedVirus(int bursts)
{
    Dictionary<(int, int), NodeStatus> tempInfected = infected.ToDictionary(x=>x,x=>NodeStatus.Infected);
    (int, int) currentNode = (0, 0);
    Direction currentDirection = Direction.Up;
    int infections = 0;

    for (int burst = 0; burst < bursts; burst++)
    {
        if (tempInfected.TryGetValue(currentNode, out NodeStatus result))
        {
            switch (result) {
                case NodeStatus.Clean: 
                    currentDirection = turnLeft(currentDirection);
                    break;
                case NodeStatus.Weakened:
                    // keep direction
                    break;
                case NodeStatus.Infected:
                    currentDirection = turnRight(currentDirection); 
                    break;
                case NodeStatus.Flagged:
                    currentDirection = turnAround(currentDirection);
                    break;
            }
            tempInfected[currentNode] = ChangeStatus(result);
            if (tempInfected[currentNode] == NodeStatus.Infected)
                infections++;
        }
        else
        {
            // Nodes not found are clean
            currentDirection = turnLeft(currentDirection);
            tempInfected[currentNode] = ChangeStatus(NodeStatus.Clean);
        }
        currentNode = move(currentNode.Item1, currentNode.Item2, currentDirection);
    }
    return infections;
}

static (int, int) move(int x, int y, Direction direction) => direction switch
{
    Direction.Up => (x, y + 1),
    Direction.Down => (x, y - 1),
    Direction.Left => (x-1, y),
    Direction.Right => (x+1, y),
    _ => throw new NotImplementedException()
};

static Direction turnLeft(Direction direction) => direction switch
{
    Direction.Up => Direction.Left,
    Direction.Left => Direction.Down,
    Direction.Down => Direction.Right,
    Direction.Right => Direction.Up,
    _ => throw new NotImplementedException()
};

static Direction turnRight(Direction direction) => direction switch
{
    Direction.Up => Direction.Right,
    Direction.Left => Direction.Up,
    Direction.Down => Direction.Left,
    Direction.Right => Direction.Down,
    _ => throw new NotImplementedException()
};

static Direction turnAround(Direction direction) => direction switch
{
    Direction.Up => Direction.Down,
    Direction.Left => Direction.Right,
    Direction.Down => Direction.Up,
    Direction.Right => Direction.Left,
    _ => throw new NotImplementedException()
};

static NodeStatus ChangeStatus(NodeStatus status) => status switch
{
    NodeStatus.Clean => NodeStatus.Weakened,
    NodeStatus.Weakened => NodeStatus.Infected,
    NodeStatus.Infected => NodeStatus.Flagged,
    NodeStatus.Flagged => NodeStatus.Clean,
    _ => throw new NotImplementedException()
};

enum Direction
{
    Up, Down, Left, Right,
}

enum NodeStatus
{
    Clean, Weakened, Flagged, Infected,
}