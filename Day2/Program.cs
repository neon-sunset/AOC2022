using System.Diagnostics;

// Part 1: Total score per format 'A X' per line with points as indicated below
// Opponent: A - Rock, B - Paper, C - Scissors
// Player: X - Rock, Y - Paper, Z - Scissors
// Points: Win +6, Draw +3, Rock +1, Paper +2, Scissors +3

// Part 2: Total score but now second argument indicates outcome
// X - Lose, Y - Draw, Z - Win

var start = Stopwatch.GetTimestamp();

var input = File.ReadAllLines("Input");
var (part1, part2) = input.Aggregate((default(int), default(int)), (scores, line) =>
{
    var (scorePart1, scorePart2) = scores;
    var (arg1, arg2) = (line[0], line[2]);

    // Part 1
    scorePart1 += CalculateScore(arg1, arg2);

    // Part 2
    // Observation: for numeric representation of ascii/u8/u16/etc. characters,
    // the losing move is --char % 3 and the winning move is ++char % 3,
    // therefore, we can treat X, Y and Z as an offset to calculate necessary choice.
    // arg1 is opponent move, arg2 is the outcome (win/lose/draw)
    var offset = (arg2 - 'Y') % 3;
    scorePart2 += CalculateScore(arg1, arg1 + offset + ('X' - 'A'));

    return (scorePart1, scorePart2);
});

Console.WriteLine($"Part1: {part1}; Part2: {part2}. Elapsed: {Stopwatch.GetElapsedTime(start).TotalMicroseconds}us\n");

// Disclaimer: I am *really* bad at math
int CalculateScore(int opponent, int player)
{
    var score = (((opponent - player) % 3) + 2) switch
    {
        2 => 6, // Win
        1 => 0, // Lose
        _ => 3 // Draw
    };

    return score + ((player + 2) % 3) + 1;
}