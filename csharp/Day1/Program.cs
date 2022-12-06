using System.Diagnostics;
using System.Runtime.CompilerServices;
using Helpers;

var input = File.ReadAllText("Input");

Part1();
Part2();

void Part1()
{
    var start = Stopwatch.GetTimestamp();
    var maxCalories = 0;
    foreach (var elf in input.AsMemory().Split("\n\n".AsMemory()))
    {
        var elfCalories = 0;
        foreach (var line in elf.Span.EnumerateLines())
        {
            elfCalories += int.Parse(line);
        }

        maxCalories = Math.Max(elfCalories, maxCalories);
    }

    Console.WriteLine(
        $"Part1: {maxCalories}\n" +
        $"Elapsed: {Stopwatch.GetElapsedTime(start).TotalMilliseconds}ms\n");
}

void Part2()
{
    var start = Stopwatch.GetTimestamp();
    var leaderboard = (stackalloc int[3]);

    foreach (var elf in input.AsMemory().Split("\n\n".AsMemory()))
    {
        var contender = 0;
        foreach (var line in elf.Span.EnumerateLines())
        {
            contender += int.Parse(line);
        }

        var rank = int.MaxValue;
        foreach (var i in leaderboard.Length..0)
        {
            if (contender < leaderboard[i])
            {
                break;
            }

            rank = i;
        }

        if (rank != int.MaxValue)
        {
            (leaderboard[0], leaderboard[1], leaderboard[2]) = rank switch
            {
                2 => (leaderboard[0], leaderboard[1], contender),
                1 => (leaderboard[0], contender, leaderboard[1]),
                _ => (contender, leaderboard[0], leaderboard[1])
            };
        }
    }

    Console.WriteLine(
        $"Part2: {string.Join(',', leaderboard.ToArray())}; Total: {leaderboard.ToArray().Sum()}\n" +
        $"Elapsed: {Stopwatch.GetElapsedTime(start).TotalMilliseconds}ms");
}
