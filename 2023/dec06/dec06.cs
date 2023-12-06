using System.Diagnostics;

var lines = File.ReadAllLines("in.txt");
var l1nums = lines[0].Split(":")[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);
var l2nums = lines[1].Split(":")[1].Split(' ', StringSplitOptions.RemoveEmptyEntries);

var part1 = Solve(
    l1nums.Select(long.Parse).ToArray(),
    l2nums.Select(long.Parse).ToArray());
Debug.Assert(part1 == 2269432);
Console.WriteLine("part 1: " + part1);

var part2 = Solve(
    new[] { long.Parse(string.Join("", l1nums)) },
    new[] { long.Parse(string.Join("", l2nums)) });
Debug.Assert(part2 == 35865985);
Console.WriteLine("part 2: " + part2);

long Solve(long[] times, long[] records)
{
    long mul = 1;
    for (int i = 0; i < times.Length; i++)
    {
        var time = times[i];
        var record = records[i];
        var sqrtD = Math.Sqrt(time * time - 4 * record);
        var x1 = (time - sqrtD) / 2;
        var x2 = (time + sqrtD) / 2;
        var numOfWays = Math.Ceiling(x2 - 1) - Math.Floor(x1 + 1) + 1;
        mul *= (int)numOfWays;
    }

    return mul;
}
