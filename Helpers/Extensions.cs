using System.Runtime.CompilerServices;

namespace Helpers;

public static class Extensions
{
    public static IEnumerable<ReadOnlyMemory<T>> Split<T>(this ReadOnlyMemory<T> source, ReadOnlyMemory<T> separator)
        where T : IEquatable<T>
    {
        int pos;
        if ((pos = source.Span.IndexOf(separator.Span)) >= 0)
        {
            do
            {
                var slice = source[..pos];
                pos += separator.Length;
                source = source[pos..];

                if (slice.Length > 0)
                {
                    yield return slice;
                }
            }
            while ((pos = source.Span.IndexOf(separator.Span)) >= 0);
        }
        else
        {
            yield return source;
        }
    }

    public static ReadOnlySplit<T> SplitFirst<T>(this ReadOnlySpan<T> source, T separator)
        where T : IEquatable<T>
    {
        ReadOnlySpan<T> left, right;

        var separatorIndex = source.IndexOf(separator);
        if (separatorIndex > -1)
        {
            left = source[..separatorIndex];
            right = source[(separatorIndex + 1)..]; // Rely on implicit slicing behaviour to not go out of bounds
        }
        else
        {
            left = source;
            right = ReadOnlySpan<T>.Empty;
        }

        return new(left, right);
    }

    public static ReadOnlySplit<T> SplitLast<T>(this ReadOnlySpan<T> source, T separator)
        where T : IEquatable<T>
    {
        ReadOnlySpan<T> left, right;

        var separatorIndex = source.LastIndexOf(separator);
        if (separatorIndex > -1)
        {
            left = source[..separatorIndex];
            right = source[(separatorIndex + 1)..]; // Rely on implicit slicing behaviour to not go out of bounds
        }
        else
        {
            left = source;
            right = ReadOnlySpan<T>.Empty;
        }

        return new(left, right);
    }

    public readonly ref struct ReadOnlySplit<T>
    {
        public readonly ReadOnlySpan<T> Left;

        public readonly ReadOnlySpan<T> Right;

        public ReadOnlySplit(ReadOnlySpan<T> left, ReadOnlySpan<T> right)
        {
            Left = left;
            Right = right;
        }

        public void Deconstruct(out ReadOnlySpan<T> left, out ReadOnlySpan<T> right)
        {
            left = Left;
            right = Right;
        }
    }
}
