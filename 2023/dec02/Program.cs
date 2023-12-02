var RED = "red"; var GREEN = "green"; var BLUE = "blue";

var games = File.ReadAllLines("in.txt")
    .Select(x =>
    {
        var parts = x.Split(":");
        var gameNumber = int.Parse(parts[0].Split(" ")[1]);
        var gameSets = parts[1].Split(";").Select(gs =>
        {
            var gameSetParts = gs.Split(",").Select(gp =>
            {
                var cubeInfos = gp.Trim().Split(" ");
                var count = int.Parse(cubeInfos[0]);
                var color = cubeInfos[1].Trim();
                return (color, count);
            }).ToArray();

            var gameSet = new Dictionary<string, int> { [RED] = 0, [GREEN] = 0, [BLUE] = 0 };
            foreach (var gameSetPart in gameSetParts)
            {
                gameSet[gameSetPart.color] = gameSetPart.count;
            }

            return gameSet;
        }).ToArray();
        return (gameNumber, gameSets);
    }).ToArray();

var part1 = games.Sum(x =>
{
    return x.gameSets.Any(x => x[RED] > 12 || x[BLUE] > 14 || x[GREEN] > 13) ? 0 : x.gameNumber;
});

var part2 = games.Sum(x =>
{
    var green = Math.Max(x.gameSets.Max(x => x[GREEN]), 1);
    var blue = Math.Max(x.gameSets.Max(x => x[BLUE]), 1);
    var red = Math.Max(x.gameSets.Max(x => x[RED]), 1);
    return red * green * blue;
});

Console.WriteLine("part1: " + part1);
Console.WriteLine("part2: " + part2);