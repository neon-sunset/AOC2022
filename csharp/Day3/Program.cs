using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;

namespace Day3;

public static class Program
{
    public static void Main()
    {
        var start = Stopwatch.GetTimestamp();

        var input = File.ReadAllLines("Input");
        var sum1 = input.Sum(line =>
        {
            var span = MemoryMarshal.Cast<char, ushort>(line);

            var commonChar = FindCommonChar(span);

            return GetPriority(commonChar);
        });

        int sum2 = 0;
        for (var i = 0; i < input.Length; i += 3)
        {
            foreach (var c in input[i])
            {
                // Maybe this is something that can be solved with bloom filter?
                // Anyways, .NET's IndexOf is vectorized so there's that.
                if (input[i + 1].IndexOf(c) != -1 &&
                    input[i + 2].IndexOf(c) != -1)
                {
                    sum2 += GetPriority(c);
                    break;
                }
            }
        }

        Console.WriteLine($"Part1: {sum1}, Part2: {sum2} Elapsed: {Stopwatch.GetElapsedTime(start).TotalMicroseconds}us");
    }

    private static char FindCommonChar(ReadOnlySpan<ushort> chars)
    {
        var length = chars.Length / 2;

        for (var i = 0; i < length / Vector128<ushort>.Count; i++)
        {
            var right = Vector128.LoadUnsafe(
                ref Unsafe.AsRef(chars[length + (i * Vector128<ushort>.Count)]));

            for (var j = 0; j < length; j++)
            {
                var mask = Vector128.Create(chars[j]);
                var result = Vector128.Equals(mask, right);
                if (result != Vector128<ushort>.Zero)
                {
                    var index = BitOperations.TrailingZeroCount(
                        result.ExtractMostSignificantBits());

                    return (char)right.GetElement(index);
                }
            }
        }

        foreach (var i in chars.Length..0)
        {
            foreach (var j in length..0)
            {
                if (chars[i] == chars[j])
                {
                    return (char)chars[i];
                }
            }
        }

        return Throw<char>();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetPriority(char c)
    {
        // I couldn't manage to extra 0x20 bit from char without using BCL :(
        var offset = char.IsAsciiLetterUpper(c) ? 27 - 'A' : 1 - 'a';

        return c + offset;
    }

    [DoesNotReturn]
    private static T Throw<T>()
    {
        throw new InvalidOperationException();
    }
}
