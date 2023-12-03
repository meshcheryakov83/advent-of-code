var lines = File.ReadAllLines("in.txt").Select(x => x + ".").ToArray();

var gears = new Dictionary<(int x, int y), List<int>>();
var neighbors = new Dictionary<(int x, int y), (int x, int y)>();
for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines[y].Length; x++)
    {
        if (lines[y][x] != '.' && !char.IsDigit(lines[y][x]))
        {
            gears[(x, y)] = new List<int>();

            neighbors[(x - 1, y - 1)] = (x, y);
            neighbors[(x, y - 1)] = (x, y);
            neighbors[(x + 1, y - 1)] = (x, y);

            neighbors[(x - 1, y)] = (x, y);
            neighbors[(x + 1, y)] = (x, y);

            neighbors[(x - 1, y + 1)] = (x, y);
            neighbors[(x, y + 1)] = (x, y);
            neighbors[(x + 1, y + 1)] = (x, y);
        }
    }
}

for (int y = 0; y < lines.Length; y++)
{
    var buf = "";
    var adjacentGears = new HashSet<(int x, int y)>();
    for (int x = 0; x < lines[y].Length; x++)
    {
        if (char.IsDigit(lines[y][x]))
        {
            buf += lines[y][x];
            if (neighbors.ContainsKey((x, y)))
            {
                adjacentGears.Add(neighbors[(x, y)]);
            }
        }
        else
        {
            if (buf.Length > 0 && adjacentGears.Count > 0)
            {
                var num = int.Parse(buf);
                foreach (var adjacentGear in adjacentGears)
                {
                    gears[adjacentGear].Add(num);
                }
            }

            adjacentGears.Clear();
            buf = "";
        }
    }
}

var part1 = gears.Sum(x => x.Value.Sum());
Console.WriteLine($"part1: {part1}");

var part2 = gears.Where(x => x.Value.Count == 2).Sum(x => x.Value[0] * x.Value[1]);
Console.WriteLine($"part2: {part2}");
