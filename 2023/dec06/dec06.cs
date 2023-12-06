Console.WriteLine("part 1: " + GetAnswer(
    new long[] { 49, 78, 79, 80 },
    new long[] { 298, 1185, 1066, 1181 }));

Console.WriteLine("part 2: " + GetAnswer(
    new long[] { 49_78_79_80 },
    new long[] { 298_1185_1066_1181 }));

long GetAnswer(long[] times, long[] prevRecords)
{
    long mul = 1;
    for (int i = 0; i < times.Length; i++)
    {
        var time = times[i];
        var prevRecord = prevRecords[i];
        var sqrtD = Math.Sqrt(time * time - 4 * prevRecord);
        var x2 = (time - sqrtD) / 2;
        var x1 = (time + sqrtD) / 2;
        var from = Math.Floor(x2) <= x2 ? Math.Floor(x2) + 1 : Math.Floor(x2);
        var to = Math.Floor(x1) >= x1 ? Math.Floor(x1) - 1 : Math.Floor(x1);
        var numOfWays = to - from + 1;
        mul *= (int)numOfWays;
    }

    return mul;
}
