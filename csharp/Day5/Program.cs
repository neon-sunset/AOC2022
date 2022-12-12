using System.Diagnostics;
using Helpers;

var start = Stopwatch.GetTimestamp();
var input = File.ReadAllLines("Input");

var createStacks = () => input
    .Take(8)
    .Reverse()
    .Aggregate(new Stack<char>[9], (stacks, line) =>
    {
        const int start = 1, offset = 4;
        foreach (var i in 0..9)
        {
            stacks[i] ??= new();

            var c = line[start + (i * offset)];
            if (char.IsAsciiLetterUpper(c))
            {
                stacks[i].Push(c);
            }
        }

        return stacks;
    });

var stacks = createStacks();
var lists = createStacks().Select(s => s.Reverse().ToList()).ToArray();

var moves = input
    .Skip(10)
    .Select(line =>
    {
        var (numChars, rest) = line.AsSpan(5).SplitFirst(' ');

        // Shift src and dst by ascii offset + 1 for zero-based indexing
        return (int.Parse(numChars), rest[5] - 49, rest[10] - 49);
    })
    .ToArray();

foreach (var (num, src, dst) in moves)
{
    foreach (var _ in 0..num)
    {
        stacks[dst].Push(stacks[src].Pop());
    }

    lists[dst].AddRange(lists[src].TakeLast(num));
    lists[src].RemoveRange(lists[src].Count - num, num);
}

var part1 = string.Join("", stacks.Select(s => s.Peek()));
var part2 = string.Join("", lists.Select(s => s.Last()));

Utils.PrintResult(start, part1, part2);