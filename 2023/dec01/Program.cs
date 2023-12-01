var digits = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
var reversed = digits.Select(Reverse).ToArray();

var lines = File.ReadAllLines("in.txt");

var sum = lines.Sum(x => FindFirst(x, digits) * 10 + FindFirst(Reverse(x), reversed));
Console.WriteLine(sum);

static string Reverse(string s) => new(s.Reverse().ToArray());

static int FindFirst(string line, string[] pattens)
{
    var dict = pattens.Select((pattern, index) => (pattern, index))
        .ToDictionary(x => x.pattern, x => x.index);
    for (int i = 0; i < line.Length; i++)
    {
        if (char.IsDigit(line[i]))
            return int.Parse(line[i].ToString());

        var val = dict.FirstOrDefault(x => line[i..].StartsWith(x.Key));
        if (val.Key != null) return val.Value;
    }

    throw new Exception("No any matches");
}
