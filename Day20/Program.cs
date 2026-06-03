using System.Text.RegularExpressions;

string[] lines = [];
try
{
    lines = File.ReadAllLines(args[0]);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
    return 1;
}

var particles = lines.Select(ParseLine);

// In the long run it's the acceleration that determine what particle stays closest to
// origin.
var accelerationMagnetude = particles.Select(p => p.Acceleration.Manhattan()).ToArray();

int shortest = accelerationMagnetude.Min();

int index = Array.IndexOf(accelerationMagnetude,shortest);


Console.WriteLine($"Part 1: {index}");
//Console.WriteLine($"Part 2: {Part2()}");




return 0;


static Particle ParseLine(string line)
{
    var result = MyRegex().Match(line.Trim());
    return new Particle(
        new Vector(int.Parse(result.Groups[1].Value), int.Parse(result.Groups[2].Value), int.Parse(result.Groups[3].Value)),
        new Vector(int.Parse(result.Groups[4].Value), int.Parse(result.Groups[5].Value), int.Parse(result.Groups[6].Value)),
        new Vector(int.Parse(result.Groups[7].Value), int.Parse(result.Groups[8].Value), int.Parse(result.Groups[9].Value))
    );
}

//int MinDistanceFromOriginSquared(Particle particle)
//{
//    return -(Vector.Scalar(particle.Position,particle.Speed))/(particle.Speed.Lenght()+Vector.Scalar(particle.Position,particle.Acceleration));
//}
//double DistanceAt(Particle p, int t)
//{
//    var x = p.Position.X + p.Speed.X * t + 0.5 * p.Acceleration.X * t * t;
//    var y = p.Position.Y + p.Speed.Y * t + 0.5 * p.Acceleration.Y * t * t;
//    var z = p.Position.Z + p.Speed.Z * t + 0.5 * p.Acceleration.Z * t * t;
//    return Math.Sqrt(x * x + y * y + z * z);
//}

record Vector(int X, int Y, int Z)
{
    //public int Lenght() => X * X + Y * Y + Z * Z;
    public int Manhattan() => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
    //public static int Scalar(Vector a, Vector b) 
    //{
    //    return (a.X*b.X)+(a.Y*b.Y)+(a.Z*b.Z);
    //}
}

record Particle(Vector Position, Vector Speed, Vector Acceleration)
{

}

partial class Program
{
    [GeneratedRegex(@"p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>")]
    private static partial Regex MyRegex();
}