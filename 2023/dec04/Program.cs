var wins = File.ReadAllLines("in.txt")
    .Select(x =>
    {
        var parts = x.Split(":|".ToCharArray());
        var targetNumbers = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();
        var numbers = parts[2].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var winNumber = numbers.Intersect(targetNumbers).Count();
        return winNumber;
    }).ToArray();

var part1 = wins.Sum(w => w > 0 ? Math.Pow(2, w - 1) : 0);
Console.WriteLine("part 1: " + part1);

var cardsCount = new List<int>();
for(int ind = 0; ind < wins.Length; ind++)
{ 
    cardsCount.Add(1 + cardsCount
        .Select((count, i) => ind - i <= wins[i] ? count : 0)
        .Sum());
}

var part2 = cardsCount.Sum();
Console.WriteLine("part 2: " + part2);