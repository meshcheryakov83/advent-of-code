using System.Diagnostics;

var lines = File.ReadAllLines("in.txt");
var instructions = lines[0];
var map = lines[2..].Select(x =>
{
    var parts = x.Split(" =(),".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
    return new[] { parts[0], parts[1], parts[2] };
}).ToDictionary(x => x[0]);

var part1 = Part1();
var part2 = Part2();
Debug.Assert($"{part1} {part2}" == File.ReadAllText("expected_out.txt").Trim());
Console.WriteLine("part1: " + part1);
Console.WriteLine("part2: " + part2);

long Part1()
{
    var cur = map["AAA"][0];
    var instrIndex = 0;

    while (cur != "ZZZ")
    {
        var instruction = instructions[instrIndex % instructions.Length];
        cur = instruction == 'L' ? map[cur][1] : map[cur][2];
        instrIndex++;
    }

    return instrIndex;
}

long Part2()
{
    var cur = map.Where(x => x.Key.EndsWith('A')).Select(x => x.Key).ToList();
    var instrIndex = 0;
    var steps = new List<int>();
    while (cur.Any())
    {
        var instruction = instructions[instrIndex % instructions.Length];
        for (int i = cur.Count-1; i >= 0; i--)
        {
            cur[i] = instruction == 'L' ? map[cur[i]][1] : map[cur[i]][2];
            if (cur[i].EndsWith('Z'))
            {
                steps.Add(instrIndex + 1);
                cur.RemoveAt(i);
            }
        }

        instrIndex++;
    }

    return steps.Aggregate((long)steps[0], (i1, i2) => i1 * i2 / GCD(i1, i2));
}

long GCD(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }

    return a;
}
