using System.Diagnostics;

var lines = File.ReadAllLines("in.txt");
var seeds = lines[0].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

var categoriesMaps = new List<List<(long destStart, long srcStart, long len)>>();
for (var i = 1; i < lines.Length; i++)
{
    if (string.IsNullOrWhiteSpace(lines[i]))
    {
        categoriesMaps.Add(new List<(long destStart, long srcStart, long len)>());
        i++;
        continue;
    }

    var nums = lines[i].Split(" ").Select(long.Parse).ToArray();
    categoriesMaps.Last().Add((nums[0], nums[1], nums[2]));
}

var part1 = Solve(seeds.SelectMany(x => new[] { x, 1 }).ToArray(), categoriesMaps);
Debug.Assert(part1 == 214922730);
Console.WriteLine($"part1: {part1}");

var part2 = Solve(seeds, categoriesMaps);
Debug.Assert(part2 == 148041808);
Console.WriteLine($"part2: {part2}");

long Solve(long[] seeds, List<List<(long destStart, long srcStart, long len)>> categoriesMaps)
{
    var ranges = Enumerable.Range(0, seeds.Length).Where(x => x % 2 == 0)
        .Select(i => new Range(seeds[i], seeds[i] + seeds[i + 1])).ToList();

    foreach (var categoryMaps in categoriesMaps)
    {
        var mappedRanges = new List<Range>();
        foreach (var map in categoryMaps)
        {
            var srcRange = new Range(map.srcStart, map.srcStart + map.len);
            var destRange = new Range(map.destStart, map.destStart + map.len);
            var shift = destRange.from - srcRange.from;

            foreach (var range in ranges.ToArray())
            {
                ranges.Remove(range);

                var parts = range.SplitWith(srcRange);

                foreach (var part in parts.Where(x => x.IsInside(range)))
                {
                    if (part.IsInside(srcRange))
                    {
                        mappedRanges.Add(new Range(part.from + shift, part.to + shift));
                    }
                    else
                    {
                        ranges.Add(part);
                    }
                }
            }
        }

        ranges.AddRange(mappedRanges);
    }

    return ranges.Min(x => x.from);
}

internal record Range(long from, long to)
{
    public IEnumerable<Range> SplitWith(Range range)
    {
        if (range.from >= to || range.to <= from) return [range, this];

        return [
            new Range(Math.Min(from, range.from), Math.Max(from, range.from)),
            new Range(Math.Max(from, range.from), Math.Min(to, range.to)),
            new Range(Math.Min(to, range.to), Math.Max(to, range.to))
        ];
    }

    public bool IsInside(Range range) => range.from <= from && to <= range.to;
};
