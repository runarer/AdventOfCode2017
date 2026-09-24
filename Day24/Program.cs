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

(int, int)[] ports = [.. lines.Select(line => line.Split('/')).Select(parts => (int.Parse(parts[0]), int.Parse(parts[1])))];

(int, int)[] startPorts = [..ports.Where(port => port.Item1 == 0 || port.Item1 == 0) ];

int highestStrength = 0;
foreach(var port in startPorts)
{
    int strength = strongestBridge([port],port.Item1 == 0 ? port.Item2 : port.Item1,ports);
    highestStrength = Math.Max(highestStrength, strength);
}

Console.WriteLine($"Part 1: {highestStrength}");

return 0;

int strongestBridge(List<(int,int)> bridge, int end, (int,int)[] parts) {
    // Strengt of currrent bridge
    int bridgeStrength = bridge.Sum(part => part.Item1 + part.Item2);
    
    //Check if we can add more part to the bridge
    foreach (var part in parts.Where(part => part.Item1 == end || part.Item2 == end))
    {
        if (bridge.Contains(part))
            continue;
        List<(int, int)> newBridge = [.. bridge,part];
        int newEnd = part.Item1 == end ? part.Item2 : part.Item1;
        int newBridgeStrength = strongestBridge(newBridge,newEnd,ports);
        bridgeStrength = Math.Max(bridgeStrength, newBridgeStrength);

    }
   
    return bridgeStrength;
}
