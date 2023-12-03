var RED = "red"; var GREEN = "green"; var BLUE = "blue";

var games = File.ReadAllLines("in.txt")
    .Select(x =>
    {
        var parts = x.Split(":");
        var gameNumber = int.Parse(parts[0].Split(" ")[1]);
        var gameSets = parts[1].Split(";").Select(gs =>
        {
            var gameSet = new Dictionary<string, int> { [RED] = 0, [GREEN] = 0, [BLUE] = 0 };
            foreach (var game in gs.Split(","))
            {
                var cubes = game.Trim().Split(" ");
                gameSet[cubes[1].Trim()] = int.Parse(cubes[0]);
            }
            return gameSet;
        }).ToArray();
        return (gameNumber, gameSets);
    }).ToArray();

var part1 = games.Sum(g => g.gameSets.Any(x => x[RED] > 12 || x[BLUE] > 14 || x[GREEN] > 13) ? 0 : g.gameNumber);

var part2 = games.Sum(g => Math.Max(g.gameSets.Max(x => x[GREEN]), 1) *
                           Math.Max(g.gameSets.Max(x => x[BLUE]), 1) *
                           Math.Max(g.gameSets.Max(x => x[RED]), 1));

Console.WriteLine("part1: " + part1);
Console.WriteLine("part2: " + part2);