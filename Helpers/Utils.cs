using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Helpers;

public static class Utils
{
    public static void PrintResult(long start, params object[] data)
    {
        var result = string.Join(", ", data);
        var elapsed = Stopwatch.GetElapsedTime(start).TotalMicroseconds;

        Console.WriteLine($"Results: {result}. Elapsed: {elapsed}us");
    }

    [DoesNotReturn]
    public static void Unreachable()
    {
        throw new InvalidOperationException();
    }

    [DoesNotReturn]
    public static T Unreachable<T>()
    {
        throw new InvalidOperationException();
    }
}