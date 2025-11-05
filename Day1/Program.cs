using System.IO;

namespace Day1
{
    internal class Program
    {
        static int? Part1(char[] chars)
        {
            int sum = chars[0] == chars[chars.Length - 1] ? chars[0] - '0' : 0;
            for (int i = 1; i < chars.Length; i++)
            {
                sum += chars[i - 1] == chars[i] ? chars[i] - '0' : 0;
            }
            return sum;
        }

        static int? Part2(char[] chars) {
            int offset = chars.Length / 2;
            
            int sum = 0;
            for (int i = 0; i < chars.Length; i++)
            {
                sum += chars[i] == chars[(i + offset)%chars.Length] ? chars[i] - '0' : 0;
            }

            return sum;
        }
        static void Main(string[] args)
        {
            using (StreamReader reader = new(args[0]))
            {
                string line = reader.ReadLine() ?? "No Lines";                
                char[] chars = line.ToCharArray();
                
                Console.WriteLine("Part 1: {0}", Part1(chars));
                Console.WriteLine("Part 2: {0}", Part2(chars));
            }
        }
    }
}
