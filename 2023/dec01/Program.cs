var digits = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
var reversedDigits = digits.Select(Reverse).ToArray();

var lines = File.ReadAllLines("in.txt");
var sum = lines.Sum(x => FindFirst(x, digits) * 10 + FindFirst(Reverse(x), reversedDigits));
Console.WriteLine(sum);

static string Reverse(string s) => new(s.Reverse().ToArray());

int FindFirst(string line, string[] pattens)
{
    for (int i = 0; i < line.Length; i++)
    {
        if (char.IsDigit(line[i]))
            return int.Parse(line[i].ToString());
    
        for (int num = 1; num < pattens.Length; num++)
        {
            var pattern = pattens[num];
            if (line.Substring(i).StartsWith(pattern))
                return num;
        }
    }
    
    throw new Exception("No first");
}