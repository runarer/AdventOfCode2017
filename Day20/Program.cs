
using System.Text.RegularExpressions;


//* There may be a need for long particle struct

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

var particles = lines.Select(ParseLine).ToList();

// In the long run it's the acceleration that determine what particle stays closest to
// origin.
var accelerationMagnetude = particles.Select(p => p.Acceleration.Manhattan()).ToArray();

int shortest = accelerationMagnetude.Min();

int index = Array.IndexOf(accelerationMagnetude,shortest);


Console.WriteLine($"Part 1: {index}");


// Part two was in the end solved by running 1000 ticks and check how many collided.
// There's a way to determine if a particle is saved.
// When the particle with the highest acceleration magnitude is
// furtherst away from origin, it's saved.
// Do not need to keep saved particles, just how many are saved.
// Sort by position from origin. 

ParticleRow[] particleRows = [..particles.Select( p => 
    new ParticleRow(
        p.Position.X,p.Position.Y,p.Position.Z,
        p.Speed.X,p.Speed.Y,p.Speed.Z,
        p.Acceleration.X,p.Acceleration.Y,p.Acceleration.Z    
    ))];

int inbound = particleRows.Count(p => p.Status == ParticleStatus.InBound);

int runs = 1_000;
while(runs > 0)
{
    CheckForCollitions(particleRows);
    MoveParticles(particleRows);
    runs--;
}

Console.WriteLine($"Part 2: {particleRows.Count(p => p.Status != ParticleStatus.Collided)}");

return 0;


//static void CheckForCollitions(ParticleRow[] rows)
//{
//    for (int i = 0; i < rows.Length - 1; i++)
//    {
//        // Skip Collided or Escaped particles.
//        if (!(rows[i].Status == ParticleStatus.InBound || rows[i].Status == ParticleStatus.OutBound))
//            break;

//        // Since the particles are sorted based on location, we check until location changes.
//        int next = i + 1;
//        while (
//            next < rows.Length && // next need to exist, be outbound or inbound, and match position
//            (rows[next].Status == ParticleStatus.OutBound || rows[next].Status == ParticleStatus.InBound) &&
//            (rows[i].Px == rows[next].Px && rows[i].Py == rows[next].Py && rows[i].Pz == rows[next].Pz))
//        {
//            rows[i].Status = ParticleStatus.Collided;
//            rows[next].Status = ParticleStatus.Collided;
//            next++;
//        }
//    }
//}

static void CheckForCollitions(ParticleRow[] rows)
{
    for (int i = 0; i < rows.Length; i++)
    {
        // Skip Collided or Escaped particles.
        if (!(rows[i].Status == ParticleStatus.InBound || rows[i].Status == ParticleStatus.OutBound))
            continue;

        for (int j = i+1; j < rows.Length; j++)
        {
            //collided or escaped particles
            if (!(rows[j].Status == ParticleStatus.InBound || rows[j].Status == ParticleStatus.OutBound))
                continue;

            if (rows[i].Px == rows[j].Px && rows[i].Py == rows[j].Py && rows[i].Pz == rows[j].Pz)
            {
                rows[i].Status = ParticleStatus.Collided;
                rows[j].Status = ParticleStatus.Collided;
            }
        }
    }
}


static void MoveParticles(ParticleRow[] rows)
{
    for (int i = 0; i < rows.Length; i++)
    {
        // Skip Collided or Escaped particles.
        if (!(rows[i].Status == ParticleStatus.InBound || rows[i].Status == ParticleStatus.OutBound))
            continue;

        // Accelerate the velocity
        rows[i].Vx += rows[i].Ax;
        rows[i].Vy += rows[i].Ay;
        rows[i].Vz += rows[i].Az;

        // Move the particle        
        rows[i].Px += rows[i].Vx;
        rows[i].Py += rows[i].Vy;
        rows[i].Pz += rows[i].Vz;

        // Update the distance
        rows[i].Dlast = rows[i].D;
        rows[i].D = Math.Abs(rows[i].Px) + Math.Abs(rows[i].Py) + Math.Abs(rows[i].Pz);

        if (rows[i].Status == ParticleStatus.InBound && rows[i].D > rows[i].Dlast)
            rows[i].Status = ParticleStatus.OutBound;
    }
}

static Particle ParseLine(string line)
{
    var result = MyRegex().Match(line.Trim());
    return new Particle(
        new Vector(int.Parse(result.Groups[1].Value), int.Parse(result.Groups[2].Value), int.Parse(result.Groups[3].Value)),
        new Vector(int.Parse(result.Groups[4].Value), int.Parse(result.Groups[5].Value), int.Parse(result.Groups[6].Value)),
        new Vector(int.Parse(result.Groups[7].Value), int.Parse(result.Groups[8].Value), int.Parse(result.Groups[9].Value))
    );
}

class Vector(int X, int Y, int Z)
{
    public int X { get; set; } = X;
    public int Y { get; set; } = Y;
    public int Z { get; set; } = Z;

    public int Manhattan() => Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
}

record Particle(Vector Position, Vector Speed, Vector Acceleration)
{
    public Vector Position { get; set; } = Position;
    public Vector Speed { get; set; } = Speed;
    public Vector Acceleration { get; set; } = Acceleration;

    public void Move()
    {
        Position.X += Speed.X;
        Position.Y += Speed.Y;
        Position.Z += Speed.Z;

        Speed.X += Acceleration.X;
        Speed.Y += Acceleration.Y;
        Speed.Z += Acceleration.Z;
    }
}

partial class Program
{
    [GeneratedRegex(@"p=<(-?\d+),(-?\d+),(-?\d+)>, v=<(-?\d+),(-?\d+),(-?\d+)>, a=<(-?\d+),(-?\d+),(-?\d+)>")]
    private static partial Regex MyRegex();
}

struct ParticleRow
{
    public ParticleRow(long px, long py, long pz, long vx, long vy, long vz, long ax, long ay, long az)
    {
        Px = px;
        Py = py;
        Pz = pz;
        Vx = vx;
        Vy = vy;
        Vz = vz;
        Ax = ax;
        Ay = ay;
        Az = az;

        // Current Distance
        D = Math.Abs(Px) + Math.Abs(Py) + Math.Abs(Pz);
        // Since acceleration is constant, we can calculate previous position
        Dlast = Math.Abs(px - (vx - ax)) + Math.Abs(py - (vy - ay)) + Math.Abs(pz - (vz - az));
        // We know current and previous position, so we can figure out if it outbound or inbound.
        Status = D < Dlast ? ParticleStatus.InBound : ParticleStatus.OutBound;
        // Acceleration magnitude
        AccM = Math.Abs(Ax) + Math.Abs(Ay) + Math.Abs(Az);
    }


    public long Px;
    public long Py;
    public long Pz;
    public long Vx;
    public long Vy;
    public long Vz;
    public readonly long Ax;
    public readonly long Ay;
    public readonly long Az;
    public long D;
    public long Dlast;
    public readonly long AccM;
    public ParticleStatus Status;

    public void Move()
    {
        Px += Vx;
        Py += Vy;
        Pz += Vz;

        Vx += Ax;
        Vy += Ay;
        Vz += Az;
    }
    public static long Distance(ParticleRow row)
    {
        return Math.Abs(row.Px) + Math.Abs(row.Py) + Math.Abs(row.Pz);
    }
}

enum ParticleStatus { InBound, OutBound, Collided, Escaped }

struct ParticleRowComparer : IComparer<ParticleRow>
{
    public readonly int Compare(ParticleRow a, ParticleRow b)
    {
        // Can add a check of status so we push rows downwards.
        bool statusA = a.Status == ParticleStatus.Collided || a.Status == ParticleStatus.Escaped;
        bool statusB = b.Status == ParticleStatus.Collided || b.Status == ParticleStatus.Escaped;

        // One is "done"
        if (statusA != statusB)
            return statusA ? 1 : -1;



        // Both are moving
        if (!statusA)
        {
            //if (a.D > b.D) return 1;
            //if (a.D < b.D) return -1;
            if (a.Px > b.Px) return 1;
            if (a.Px < b.Px) return -1;
            if (a.Py > b.Py) return 1;
            if (a.Py < b.Py) return -1;
            if (a.Pz > b.Pz) return 1;
            if (a.Pz < b.Pz) return -1;
        }

        return 0;
    }
}