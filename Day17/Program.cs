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

Console.WriteLine($"Part 1: {Solution(steps,2018,2017)}");
Console.WriteLine($"Part 1: {Solution(steps,50_000_000,0)}");
return 0;

static int Solution(int steps, int ValuesToAdd,int after)
{
    
    int[] list = new int[ValuesToAdd];    
    list[0] = 1;
    list[1] = 0;
    int currentNode = 1;
    int currentSize = 2;

    while (currentSize < ValuesToAdd)
    {
        int movesForward = steps % currentSize;
        while (movesForward > 0)
        {
            currentNode = list[currentNode];
            movesForward--;
        }

        list[currentSize] = list[currentNode];
        list[currentNode] = currentSize;
        currentNode = currentSize;
        currentSize++;        
    }
        
    return list[after];
}
