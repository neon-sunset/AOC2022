namespace Helpers;

public static class Extensions
{
    public static IEnumerable<Memory<T>> Split<T>(this Memory<T> source, ReadOnlyMemory<T> separator)
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
}
