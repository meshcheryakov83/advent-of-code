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

            var gameSet = new Dictionary<string, int> { ["red"] = 0, ["green"] = 0, ["blue"] = 0 };
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
    return x.gameSets.Any(x => x["red"] > 12 || x["blue"] > 14 || x["green"] > 13) ? 0 : x.gameNumber;
});

var part2 = games.Sum(x =>
{
    var green = Math.Max(x.gameSets.Max(x => x["green"]), 1);
    var blue = Math.Max(x.gameSets.Max(x => x["blue"]), 1);
    var red = Math.Max(x.gameSets.Max(x => x["red"]), 1);
    return red * green * blue;
});

Console.WriteLine("part1: " + part1);
Console.WriteLine("part2: " + part2);