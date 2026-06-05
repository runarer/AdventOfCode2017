
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

int runs = 1_000;
while(runs > 0)
{
    Array.Sort(particleRows, new ParticleRowComparer());
    CheckForCollitions(particleRows);
    MoveParticles(particleRows);
    runs--;
}






//int runs = 10_000;
//int particlesSaved = 0;
////int particlesLeft = particles.Count;
//while(particles.Count > 1 || runs < 1)
//{
//    // sort by distance to origin
//    //particles.Sort(CompareParticles);
//    //particles = particles.OrderBy(p => p.Position.Manhattan()).ThenBy(p => p.Position.X).ThenBy(p => p.Position.Y).ThenBy(p => p.Position.Y).ToList();
//    particles.Sort((a,b) => a.Position.Manhattan().CompareTo(b.Position.Manhattan()));
//    //Array.Sort(particles, CompareParticles);

//    // Resolve any collitions
//    RemoveParticleCollitions(particles);
//    if (particles.Count <= 1)
//    { 
//        particlesSaved += particles.Count;
//        break; 
//    }

//    // Resolve escaped particles
//    //particlesSaved += RemoveEscapedParticles(particles);
//    //if (particles.Count <= 1)
//    //{
//    //    break;
//    //}

//    // Move particles
//    foreach (var particle in particles)
//        particle.Move();

//    runs--;
//}



//Console.WriteLine($"Part 2: {particlesSaved}");


return 0;


static void CheckForCollitions(ParticleRow[] rows)
{
    for(int i = 0; i < rows.Length-1; i++)
    {
        // Skip Collided or Escaped particles.
        if (!(rows[i].Status == ParticleStatus.InBound || rows[i].Status == ParticleStatus.OutBound))
            break;

        // Since the particles are sorted based on location, we check until location changes.
        int next = i + 1;
        while (
            next < rows.Length && // next need to exist, be outbound or inbound, and match position
            (rows[next].Status == ParticleStatus.OutBound || rows[next].Status == ParticleStatus.InBound) &&
            (rows[i].Px == rows[next].Px && rows[i].Py == rows[next].Py && rows[i].Pz == rows[next].Pz))
        {
            rows[i].Status = ParticleStatus.Collided;
            rows[next].Status = ParticleStatus.Collided;
            next++;
        }
    }
}

static void MoveParticles(ParticleRow[] rows)
{
    for (int i = 0; i < rows.Length; i++)
    {
        // Skip Collided or Escaped particles.
        if (!(rows[i].Status == ParticleStatus.InBound || rows[i].Status == ParticleStatus.OutBound))
            break;

        // Move the particle        
        rows[i].Px += rows[i].Vx;
        rows[i].Py += rows[i].Vy;
        rows[i].Pz += rows[i].Vz;

        // Accelerate the velocity
        rows[i].Vx += rows[i].Ax;
        rows[i].Vy += rows[i].Ay;
        rows[i].Vz += rows[i].Az;

        // Update the distance
        rows[i].Dlast = rows[i].D;
        rows[i].D = Math.Abs(rows[i].Px) + Math.Abs(rows[i].Py) + Math.Abs(rows[i].Pz);

        if (rows[i].Status == ParticleStatus.InBound && rows[i].D > rows[i].Dlast)
            rows[i].Status = ParticleStatus.OutBound;
    }
}


//static int CompareParticles(Particle a, Particle b)
//{
//    if (a.Position.X > b.Position.X) return 1;
//    if (a.Position.X < b.Position.X) return -1;
//    if (a.Position.Y > b.Position.Y) return 1;
//    if (a.Position.Y < b.Position.Y) return -1;
//    if (a.Position.Z > b.Position.Z) return 1;
//    if (a.Position.Z < b.Position.Z) return -1;
//    return 0;
//}

//static int CompareParticleRows(ParticleRow a, ParticleRow b)
//{
//    // Can add a check of status so we push rows downwards.
//    bool statusA = a.Status == ParticleStatus.Collided || a.Status == ParticleStatus.Escaped;
//    bool statusB = b.Status == ParticleStatus.Collided || b.Status == ParticleStatus.Escaped;

//    // One is "done"
//    if(statusA != statusB)
//        return statusA ? 1 : -1;

//    // Both are moving
//    if(!statusA)
//    {
//        if (a.Px > b.Px) return 1;
//        if (a.Px < b.Px) return -1;
//        if (a.Py > b.Py) return 1;
//        if (a.Py < b.Py) return -1;
//        if (a.Pz > b.Pz) return 1;
//        if (a.Pz < b.Pz) return -1;
//    }

//    return 0;
//}




//static void RemoveParticleCollitions(List<Particle> particles)
//{

//    // foreach particle we need to check it againt all other paricles with 
//    List<int> remove = [];
//    for (int i = 0; i < particles.Count - 1; i++)
//    {
//        if (particles[i].Position.X == particles[i + 1].Position.X
//         && particles[i].Position.Y == particles[i + 1].Position.Y
//         && particles[i].Position.Z == particles[i + 1].Position.Z)
//        {
//            int removeFrom = i;
//            int removeTo = i + 1;
//            remove.Add(removeFrom);
//            remove.Add(removeTo);
//            while (removeTo < particles.Count - 1 && particles[i].Position.X == particles[removeTo + 1].Position.X
//               && particles[i].Position.Y == particles[removeTo + 1].Position.Y
//               && particles[i].Position.Z == particles[removeTo + 1].Position.Z)
//            {
//                remove.Add(++removeTo);
//            }
//        }
//    }
//    remove.Sort((a,b) => b.CompareTo(a));
//    foreach (int i in remove)
//        particles.RemoveAt(i);
//}

//static void RemoveParticleCollitions(List<Particle> particles)
//{
//    List<int> remove = [];

//    for(int i = 0; i < particles.Count; i++)
//    {
//        int distance = particles[i].Position.Manhattan();
//        int compareTo = i + 1;

//        // Aslong as there is particles thats the same distance, compare them 
//        while (compareTo < particles.Count && particles[compareTo].Position.Manhattan() == distance)
//        {
//            if (particles[i].Position.X == particles[i + 1].Position.X
//             && particles[i].Position.Y == particles[i + 1].Position.Y
//             && particles[i].Position.Z == particles[i + 1].Position.Z)
//            {
//                // We have a collition, lets add to remove list, if not there allready
//                if(!remove.Contains(i)) 
//                    remove.Add(i);
//                if(!remove.Contains(compareTo)) 
//                    remove.Add(compareTo);
//            }

//            compareTo++;
//        }
//    }

//    remove.Sort((a,b) => b.CompareTo(a));
//    foreach (int i in remove)
//        particles.RemoveAt(i);
//}

//static int RemoveEscapedParticles(List<Particle> particles)
//{
//    int saved = 0;
//    // need to know max acceleration magnitude
//    int maxAcceleration = particles.Max(p => p.Acceleration.Manhattan());
//    Particle particle = particles.Last();
//    if (particle.Acceleration.Manhattan() == maxAcceleration)
//    {
//        saved++;
//        particles.Remove(particle);
//    }

//    return saved;
//}


//int RemoveParticles(List<(int,int)> remove, Particle[] particles, int particlesLeft)
//{
//    int removed = 0;

//    int removeFrom = remove[removed].Item1;
//    int replaceWith = remove[removed].Item2+1;
//    // If there's any particles to move up, do it
//    if(!replaceWith >= particlesLeft )
//    {

//    }


//    // sums up how many particles where removed.. 
//    return particlesLeft - remove.Sum( a => a.Item2 - a.Item1 + 1);
//}


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

//static bool Collide(Particle particleA, Particle particleB)
//{
//    var (tx1,tx2) = Quadratic(particleA.Acceleration.X - particleB.Acceleration.X, 
//                              particleA.Speed.X - particleB.Speed.X, 
//                              particleA.Position.X - particleB.Position.X);
//    if (tx1 is null && tx2 is null)
//        return false;

//    var (ty1, ty2) = Quadratic(particleA.Acceleration.Y - particleB.Acceleration.Y,
//                               particleA.Speed.Y - particleB.Speed.Y,
//                               particleA.Position.Y - particleB.Position.Y);
//    if (ty1 is null && ty2 is null)
//        return false;
    
//    var (tz1, tz2) = Quadratic(particleA.Acceleration.Z - particleB.Acceleration.Z,
//                               particleA.Speed.Z - particleB.Speed.Z,
//                               particleA.Position.Z - particleB.Position.Z);
//    if (tz1 is null && tz2 is null)
//        return false;


//    double tolerance = 0.00001;

//    if (tx1.HasValue)    
//        // Har vi en match med y?
//        if((ty1.HasValue && Math.Abs(ty1.Value - tx1.Value) > tolerance) || (ty2.HasValue && Math.Abs(ty2.Value - tx1.Value) > tolerance))        
//            // Har vi en match med z
//            if ((tz1.HasValue && Math.Abs(tz1.Value - tx1.Value) > tolerance) || (tz2.HasValue && Math.Abs(tz2.Value - tx1.Value) > tolerance))
//                return true;
//    if (tx2.HasValue)
//        // Har vi en match med y?
//        if ((ty1.HasValue && Math.Abs(ty1.Value - tx2.Value) > tolerance) || (ty2.HasValue && Math.Abs(ty2.Value - tx2.Value) > tolerance))
//            // Har vi en match med z
//            if ((tz1.HasValue && Math.Abs(tz1.Value - tx2.Value) > tolerance) || (tz2.HasValue && Math.Abs(tz2.Value - tx2.Value) > tolerance))
//                return true;
//    return false;
//}




//static (double?,double?) Quadratic(int a, int b, int c)
//{
//    if (a == 0)
//    {
//        return (null,null);
//    }

//    // Calculate the discriminant (b^2 - 4ac)
//    double discriminant = (b * b) - (4 * a * c);

//    if (discriminant > 0)
//    {
//        // Two distinct real roots
//        double root1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
//        double root2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
//        return (root1 > 0 ? root1 : null,root2 > 0 ? root2 : null);
//    }
//    else if (discriminant == 0)
//    {
//        double root = -b / (2 * a);
//        return (root > 0 ? root : null, null);
//    }
//    return (null, null);
//}

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
    public ParticleRow(int px, int py, int pz, int vx, int vy, int vz, int ax, int ay, int az)
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
        Dlast = Math.Abs(px - (vx-ax)) + Math.Abs(py - (vy-ay)) + Math.Abs(pz - (vz-az));
        // We know current and previous position, so we can figure out if it outbound or inbound.
        Status = D < Dlast ? ParticleStatus.InBound : ParticleStatus.OutBound;
        // Acceleration magnitude, 
        AccM = Math.Abs(Ax) + Math.Abs(Ay) + Math.Abs(Ay);
    }


    public int Px;
    public int Py;
    public int Pz;
    public int Vx;
    public int Vy;
    public int Vz;
    public readonly int Ax;
    public readonly int Ay;
    public readonly int Az;
    public int D;
    public int Dlast;
    public readonly int AccM;
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
    public static int Distance(ParticleRow row)
    {
        return Math.Abs(row.Px) + Math.Abs(row.Py) + Math.Abs(row.Py);
    }
}

enum ParticleStatus { InBound, OutBound, Collided, Escaped }

struct ParticleRowComparer : IComparer<ParticleRow>
{
    public int Compare(ParticleRow a, ParticleRow b)
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