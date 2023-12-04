var wins = File.ReadAllLines("in.txt")
    .Select(x =>
    {
        int[] parseNumbers(string str) => str.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        var parts = x.Split(":|".ToCharArray());
        var targetNumbers = parseNumbers(parts[1]);
        var numbers = parseNumbers(parts[2]);
        var winNumber = numbers.Intersect(targetNumbers).Count();
        return winNumber;
    }).ToArray();

var part1 = wins.Where(w => w > 0).Sum(w => Math.Pow(2, w - 1));
Console.WriteLine("part 1: " + part1);

var cardsCount = new List<int>();
for (int ind = 0; ind < wins.Length; ind++)
{
    cardsCount.Add(1 + cardsCount.Where((_, i) => ind - i <= wins[i]).Sum());
}
var part2 = cardsCount.Sum();
Console.WriteLine("part 2: " + part2);