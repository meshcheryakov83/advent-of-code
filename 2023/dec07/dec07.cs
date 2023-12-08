using System.Diagnostics;

var suffix = "";
var cards = File.ReadAllLines($"{suffix}in.txt")
    .Select(line => new Hand(line[..5], int.Parse(line[6..]))).ToArray();

var part1 = CalcTotal(null, "23456789TJQKA");
Console.WriteLine($"part1: {part1}");

var part2 = CalcTotal(PreprocessJoker, "J23456789TQKA");
Console.WriteLine($"part2: {part2}");

Debug.Assert($"{part1} {part2}" == File.ReadAllText($"{suffix}out.txt").Trim());

// move joker to first max bucket
void PreprocessJoker(Dictionary<char, int> buckets)
{
    var jCount = buckets.GetValueOrDefault('J', 0);
    buckets.Remove('J');
    if (!buckets.Any())
    {
        buckets['J'] = 0;
    }
    buckets[buckets.MaxBy(x => x.Value).Key] += jCount;
}

long CalcTotal(Action<Dictionary<char, int>>? processBuckets, string cardIndexes)
{
    return cards.OrderBy(x => CalcHandWeight(x.cards, processBuckets, cardIndexes))
        .Select((x, i) => (x.bid, i))
        .Aggregate(0, (sum, hand) => sum + hand.bid * (hand.i + 1));
}

long CalcHandWeight(string hand, Action<Dictionary<char, int>>? processBuckets, string cardIndexes)
{
    var buckets = hand.ToCharArray().ToHashSet().ToDictionary(ch => ch, ch => hand.Count(x => x == ch));
    processBuckets?.Invoke(buckets);
    var strength = (long) (buckets.MaxBy(x => x.Value).Value switch
    {
        5 => Types.FiveOfAKind,
        4 => Types.FourOfAKind,
        3 => buckets.Count == 2 ? Types.FullHouse : Types.ThreeOfAKind,
        2 => buckets.Count == 3 ? Types.TwoPair : Types.OnePair,
        _ => Types.HighCard
    });

    return hand.Aggregate(strength, (current, ch) => current * 100 + cardIndexes.IndexOf(ch));
}

internal record Hand(string cards, int bid);

public enum Types
{
    HighCard,
    OnePair,
    TwoPair,
    ThreeOfAKind,
    FullHouse,
    FourOfAKind,
    FiveOfAKind
}
