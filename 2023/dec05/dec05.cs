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

                // [ ] ( ) or ( ) [ ] - no intersections - return range to ranges as is
                if (range.from >= srcRange.to || range.to <= srcRange.from)
                {
                    ranges.Add(range);
                }
                // [ (  ]  ) - map intersection and other part return to ranges collection as is
                else if (range.from >= srcRange.from && range.from < srcRange.to && range.to > srcRange.to)
                {
                    mappedRanges.Add(new Range(range.from + shift, srcRange.to + shift));
                    ranges.Add(new Range(srcRange.to, range.to));
                }
                // [ (    ) ] - map intersection
                else if (range.from >= srcRange.from && range.to <= srcRange.to)
                {
                    mappedRanges.Add(new Range(range.from + shift, range.to + shift));
                }
                // ( [ )  ] - return non intersection part to ranges as is and map another part
                else if (range.from < srcRange.from && range.to > srcRange.from && range.to < srcRange.to)
                {
                    ranges.Add(new Range(range.from, srcRange.from));
                    mappedRanges.Add(new Range(srcRange.from + shift, range.to + shift));
                }
                // ( [   ] ) - two parts at the begining and at end return as is and map part in the middle
                else if (range.from < srcRange.from && range.to > srcRange.to)
                {
                    ranges.Add(new Range(range.from, srcRange.from));
                    mappedRanges.Add(new Range(destRange.from, destRange.to));
                    ranges.Add(new Range(srcRange.to, range.to));
                }
            }
        }
        // after applying all the mappings from category, put newly mapped ranges into the ranges
        ranges.AddRange(mappedRanges);
    }

    return ranges.Min(x => x.from);
}

internal record Range(long from, long to);