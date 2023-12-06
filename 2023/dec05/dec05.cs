using System.Diagnostics;

var lines = File.ReadAllLines("in.txt");
var seeds = lines[0].Split(":")[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();

var categories = new List<List<(long destStart, long srcStart, long len)>>();
for (int i = 1; i < lines.Length; i++)
{
    if (string.IsNullOrWhiteSpace(lines[i]))
    {
        categories.Add(new List<(long destStart, long srcStart, long len)>());
        i++;
        continue;
    }

    var nums = lines[i].Split(" ").Select(long.Parse).ToArray();
    categories.Last().Add((nums[0], nums[1], nums[2]));
}

var locations = new List<long>();
foreach (var seed in seeds)
{
    var src = seed;
    foreach (var category in categories)
    {
        foreach (var map in category)
        {
            if (src >= map.srcStart && src <= map.srcStart + map.len)
            {
                src = map.destStart + (src - map.srcStart);
                break;
            }
        }
    }

    locations.Add(src);
}
var part1 = locations.Min();
Debug.Assert(part1 == 214922730);
Console.WriteLine("part1: " + part1);

List<Range> ranges = new();
for (int i = 0; i < seeds.Length; i += 2)
{
    ranges.Add(new Range(seeds[i], seeds[i] + seeds[i + 1]));
}

foreach (var category in categories)
{
    var added = new List<Range>();
    foreach (var map in category)
    {
        var srcRange = new Range(map.srcStart, map.srcStart + map.len);
        var destRange = new Range(map.destStart, map.destStart + map.len);
        var shift = destRange.from - srcRange.from;
        
        foreach (var range in ranges.ToArray())
        {
            ranges.Remove(range);

            // [ ] ( )
            if (range.from >= srcRange.to || range.to <= srcRange.from)
            {
                ranges.Add(range);
            }
            // [ (  ]  )
            else if (range.from >= srcRange.from && range.from < srcRange.to && range.to > srcRange.to)
            {
                added.Add(new Range(range.from + shift, srcRange.to + shift));
                ranges.Add(new Range(srcRange.to, range.to));
            }
            // [ (    ) ]
            else if (range.from >= srcRange.from && range.to <= srcRange.to)
            {
                added.Add(new Range(range.from + shift, range.to + shift));
            }
            // ( [ )  ]
            else if (range.from < srcRange.from && range.to > srcRange.from && range.to < srcRange.to)
            {
                ranges.Add(new Range(range.from, srcRange.from));
                added.Add(new Range(srcRange.from + shift, range.to + shift));
            }
            // ( [   ] )
            else if (range.from < srcRange.from && range.to > srcRange.to)
            {
                ranges.Add(new Range(range.from, srcRange.from));
                added.Add(new Range(destRange.from, destRange.to));
                ranges.Add(new Range(srcRange.to, range.to));
            }
        }
    }

    ranges.AddRange(added);
}

var part2 = ranges.Min(x=>x.from);
Console.WriteLine("part2: " + part2);
Debug.Assert(part2 == 148041808);

record Range(long from, long to);