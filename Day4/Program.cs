using System.Diagnostics;
using Helpers;

var start = Stopwatch.GetTimestamp();

var (fullOverlapCount, partialOverlapCount) = File
    .ReadAllLines("Input")
    .Aggregate((Full: 0, Partial: 0), (count, line) =>
    {
        var split = line.AsSpan().SplitFirst(',');

        var (x1, x2) = ParsePair(split.Left);
        var (y1, y2) = ParsePair(split.Right);

        // It's the job of the compiler to be good at optimizing math >:|
        var full = x1 == y1
            || (x1 < y1 ? x2 >= y2 : x2 <= y2);

        var partial = full
            || (x1 <= y2 && y1 <= x2);

        return (full, partial) switch
        {
            (true, _) => (++count.Full, ++count.Partial),
            (_, true) => (count.Full, ++count.Partial),
            _ => count
        };
    });

Utils.PrintResult(start, fullOverlapCount, partialOverlapCount);

static (int, int) ParsePair(ReadOnlySpan<char> source)
{
    var (start, end) = source.SplitFirst('-');

    return (int.Parse(start), int.Parse(end));
}
